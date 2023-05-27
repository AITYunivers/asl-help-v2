using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AslHelp.Common.Exceptions;
using AslHelp.Core.Reflection;
using Irony.Parsing;
using LiveSplit.ASL;
using LiveSplit.UI.Components;

namespace AslHelp.Core.LiveSplitInterop;

internal static class Script
{
    private static ASLScript _script;
    private static ASLComponent _component;

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
            if (methods.FirstOrDefault() is not ASLMethod method)
            {
                return false;
            }

            object cc = method.GetFieldValue<object>("_compiled_code");
            return cc.GetType().Assembly == ReflectionExtensions.CurrentAssembly;
        });

        if (component is null)
        {
            Debug.Error("    => Failure! ASLComponent could not be found.");
            ThrowHelper.ThrowInvalidOperationException("ASLComponent not found.");
        }

        _component = component;
        _script = _component.Script;

        Vars = _script.Vars;
        SettingsBuilder = _script.GetFieldValue<ASLSettings>("_settings").Builder;

        Actions.Init(_script);
        LoadMethods();

        Debug.Info("    => Success.");
    }

    public static Process Game
    {
        get => Emissions.GetScriptGame(_script);
        set => Emissions.SetScriptGame(_script, value);
    }

    public static IDictionary<string, object> Vars { get; private set; }
    public static IDictionary<string, object> Current => _script.State.Data;

    public static ASLSettingsBuilder SettingsBuilder { get; private set; }

    private static void LoadMethods()
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
