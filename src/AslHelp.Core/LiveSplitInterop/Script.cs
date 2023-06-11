#nullable disable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;

using Irony.Parsing;

using LiveSplit.ASL;
using LiveSplit.UI.Components;

namespace AslHelp.Core.LiveSplitInterop;

/// <summary>
///     The <see cref="Script"/> class
///     exposes a few LiveSplit auto splitter script-related properties.
/// </summary>
/// <remarks>
///     An instance of asl-help should always be uniquely tied to one script. This class is therefore
///     marked as <see langword="static"/>. Call <see cref="Init"/> to initialize the relevant data.
/// </remarks>
internal static class Script
{
    private static ASLScript _script;
    private static ASLComponent _component;

    private static IDictionary<string, object> _current;

    /// <summary>
    ///     Retrieves the <see cref="ASLScript"/> that loaded this instance of asl-help and retrieves its data.
    /// </summary>
    /// <remarks>
    ///     This method can only succeed when this instance of asl-help was in fact loaded by an auto splitter script.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the auto splitter script that loaded this instance of asl-help could not be found.
    /// </exception>
    public static void Init()
    {
        Debug.Info("  => Initializing script data...");

        IEnumerable<IComponent> components = Timer.Layout.Components.Prepend(Timer.Run.AutoSplitter?.Component);

        ASLComponent component = (ASLComponent)components.FirstOrDefault(c =>
        {
            if (c is not ASLComponent aslc)
            {
                return false;
            }

            if (aslc.Script is not ASLScript script)
            {
                return false;
            }

            ASLScript.Methods methods = script.GetFieldValue<ASLScript.Methods>("_methods");
            if (methods?.FirstOrDefault() is not ASLMethod method)
            {
                return false;
            }

            object cc = method.GetFieldValue<object>("_compiled_code");
            return cc?.GetType().Assembly == ReflectionExtensions.CurrentAssembly;
        });

        if (component is null)
        {
            Debug.Error("    => Failure! ASLComponent could not be found.");
            ThrowHelper.ThrowInvalidOperationException("ASLComponent not found.");
        }

        _component = component;
        _script = component.Script;

        Vars = _script.Vars;
        SettingsBuilder = _script.GetFieldValue<ASLSettings>("_settings").Builder;

        Actions.Init(_script);
        LoadActions();

        Debug.Info("    => Success.");
    }

    /// <summary>
    ///     Gets the script's `vars` object.
    /// </summary>
    public static IDictionary<string, object> Vars { get; private set; }

    /// <summary>
    ///     Gets the script's `current` object.
    /// </summary>
    public static IDictionary<string, object> Current => _current ??= _script.State.Data;

    /// <summary>
    ///     Gets the script's <see cref="ASLSettingsBuilder"/>.
    /// </summary>
    public static ASLSettingsBuilder SettingsBuilder { get; private set; }

    /// <summary>
    ///     Mocks LiveSplit's ASL script parser and retrieves the script's actions and their contents.
    /// </summary>
    private static void LoadActions()
    {
        Debug.Info("    => Parsing existing methods...");

        ComponentSettings settings = _component.GetFieldValue<ComponentSettings>("_settings");
        string path = settings.ScriptPath;
        string code = File.ReadAllText(path);

        ASLGrammar grammar = new();
        Parser parser = new(grammar);

        ParseTree tree = parser.Parse(code);
        ParseTreeNode node = tree.Root.ChildNodes.First(n => n.Term.Name == "methodList");

        foreach (ParseTreeNode method in node.ChildNodes[0].ChildNodes)
        {
            string name = (string)method.ChildNodes[0].Token.Value;
            string body = (string)method.ChildNodes[2].Token.Value;
            int line = method.ChildNodes[0].Token.Location.Line + 1;
            typeof(Actions).SetPropertyValue<Actions.Action>(name, new(body, name, line));
        }

        Debug.Info("      => Success.");
    }
}
