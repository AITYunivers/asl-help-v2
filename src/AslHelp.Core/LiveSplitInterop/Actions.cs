#nullable disable

using System;
using System.Diagnostics;
using System.Reflection;

using AslHelp.Common.Extensions;

using LiveSplit.ASL;

namespace AslHelp.Core.LiveSplitInterop;

/// <summary>
///     The <see cref="Actions"/> class
///     contains the actions as they were defined in the <see cref="ASLScript"/>.
/// </summary>
internal static class Actions
{
    private static ASLScript.Methods _methods;

    /// <summary>
    ///     Retrieves an <see cref="ASLScript"/>'s actions.
    /// </summary>
    public static void Init(ASLScript script)
    {
        _methods = script.GetFieldValue<ASLScript.Methods>("_methods");
    }

    /// <summary>
    ///     Finds the name of the action which this currently being executed.<br/>
    ///     This is done by traversing the stack trace and finding which method in the <see cref="ASLScript"/> we are being called from.
    /// </summary>
    public static string CurrentAction
    {
        get
        {
            StackFrame[] frames = new StackTrace(6, false).GetFrames();

            for (int i = 0; i < frames.Length; i++)
            {
                StackFrame frame = frames[i];
                MethodBase method = frame.GetMethod();

                if (method.DeclaringType != typeof(ASLScript))
                {
                    continue;
                }

                string name = method.Name;
                if (name == "RunMethod")
                {
                    continue;
                }

                int j = char.IsUpper(name[2]) ? 2 : 3;
                return name[j..].ToLower();
            }

            throw new Exception("Not in an ASL.");
        }
    }

#pragma warning disable IDE1006
    public static Action startup { get; set; } = new("startup");
    public static Action shutdown { get; set; } = new("shutdown");
    public static Action init { get; set; } = new("init");
    public static Action exit { get; set; } = new("exit");
    public static Action update { get; set; } = new("update");
    public static Action start { get; set; } = new("start");
    public static Action split { get; set; } = new("split");
    public static Action reset { get; set; } = new("reset");
    public static Action gameTime { get; set; } = new("gameTime");
    public static Action isLoading { get; set; } = new("isLoading");
    public static Action onStart { get; set; } = new("onStart");
    public static Action onSplit { get; set; } = new("onSplit");
    public static Action onReset { get; set; } = new("onReset");
#pragma warning restore IDE1006

    internal record struct Action(
        string Body,
        string Name,
        int Line)
    {
        public Action(string name)
            : this("", name, 0) { }

        public void Append(string code)
        {
            Body = $"{Body}{code}";
            Update();

            Debug.Info($"    => Added the following code to the end of '{Name}':");
            Debug.Info($"       `{code}`");
        }

        public void Prepend(string code)
        {
            Body = $"{code}{Body}";
            Update();

            Debug.Info($"    => Added the following code to the beginning of '{Name}':");
            Debug.Info($"       `{code}`");
        }

        private readonly void Update()
        {
            ASLMethod method = new(Body, Name, Line);
            _methods.SetFieldValue(Name, method);
        }
    }
}
