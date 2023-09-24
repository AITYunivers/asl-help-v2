using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.LiveSplitInterop;

public partial class Basic
{
    protected sealed override void InitImpl()
    {
        AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        AppDomain.CurrentDomain.FirstChanceException += FirstChanceHandler;

        _logger.Add(new DebugLogger());
    }

    protected override void GenerateCodeImpl(string? helperName)
    {
        Script.Vars["Log"] = (Action<object>)(output => Logger.Log($"[{GameName}] {output}"));
        Debug.Info("    => Created the Action<object> `vars.Log`.");

        if (helperName is not null)
        {
            Actions.exit.Prepend($"vars.{helperName}.{nameof(OnExit)}();");
            Actions.shutdown.Prepend($"vars.{helperName}.{nameof(OnShutdown)}();");
        }
        else
        {
            Debug.Warn("    => Helper was not found as part of 'vars'.");
            Debug.Warn("    => Make sure to call `OnExit` and `OnShutdown` in their respective actions manually.");
        }
    }

    private static Assembly? AssemblyResolve(object sender, ResolveEventArgs e)
    {
        string name = e.Name;
        int i = name.IndexOf(',');
        if (i == -1)
        {
            ThrowHelper.ThrowArgumentException(nameof(e.Name), "Assembly name was in an unexpected format.");
        }

        string file = $"Components/{name[..i]}.dll";

        return File.Exists(file) ? Assembly.LoadFrom(file) : null;
    }

    private static readonly FieldInfo _messageField = typeof(Exception).GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static void FirstChanceHandler(object source, FirstChanceExceptionEventArgs e)
    {
        StackFrame[] frames = new StackTrace(1, false).GetFrames();

        int last = frames.Length - 1;
        for (; last >= 0; last--)
        {
            MethodBase? mb = frames[last].GetMethod();
            if (mb?.DeclaringType?.Assembly == ReflectionExtensions.ExecutingAssembly)
            {
                break;
            }
        }

        if (last == -1)
        {
            return;
        }

        Exception ex = e.Exception;
        string message = $"""
            {string.Join("\n", ex.Message.Split('\n').Select(l => l.TrimEnd()).Distinct())}
            {string.Join("\n", frames.Take(last + 1)
                .Select(f =>
                {
                    MethodBase? mb = f.GetMethod();
                    string declaration = $"{mb}";
                    int space = declaration.IndexOf(' ') + 1;

                    return $"   at {mb?.DeclaringType}.{declaration[space..]}";
                }))}
            """;

        _messageField.SetValue(ex, $"{message}".Trim());
    }
}
