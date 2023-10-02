using System;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;

using AslHelp.Common.Exceptions;
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

    private const string UpdateAndExecuteVoid =
        "   at System.Dynamic.UpdateDelegates.UpdateAndExecuteVoid2[T0,T1](CallSite site, T0 arg0, T1 arg1)";
    private const string CompiledScriptExecute =
        "   at CompiledScript.Execute(LiveSplitState timer, Object old, Object current, Object vars, Process game, Object settings)";

    private static readonly string[] _newLines = ["\r\n", "\n"];
    private static readonly FieldInfo _messageField = typeof(Exception).GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static void FirstChanceHandler(object source, FirstChanceExceptionEventArgs e)
    {
        if (e.Exception is not LiveSplit.ASL.ASLRuntimeException ex)
        {
            return;
        }

        if (ex.Message is not string message)
        {
            return;
        }

        if (ex.InnerException?.StackTrace is not string stackTrace)
        {
            return;
        }

        string[] messageLines = message.Split(_newLines, StringSplitOptions.None);
        string[] stackTraceLines = stackTrace.Split(_newLines, StringSplitOptions.None);

        StringBuilder sb = new(message.Length + stackTrace.Length);

        for (int i = 0; i < messageLines.Length - 2; i++)
        {
            sb.AppendLine(messageLines[i]);
        }

        foreach (string line in stackTraceLines)
        {
            if (line.StartsWith(UpdateAndExecuteVoid) || line.StartsWith(CompiledScriptExecute))
            {
                break;
            }

            sb.AppendLine(line);
        }

        sb.AppendLine();
        sb.Append(messageLines[^1]);

        _messageField.SetValue(ex, sb.ToString());
    }
}
