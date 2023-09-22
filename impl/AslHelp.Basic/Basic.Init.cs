using System;
using System.IO;
using System.Reflection;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.LiveSplitInterop;

using LsTimer = AslHelp.Core.LiveSplitInterop.Timer;

public partial class Basic
{
    protected sealed override void InitImpl()
    {
        AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;

        _logger.Add(new DebugLogger());

        LsTimer.Init();
        Script.Init();
    }

    protected override void GenerateCode()
    {
        string? helperName = null;

        foreach (var entry in Script.Vars)
        {
            if (entry.Value == this)
            {
                helperName = entry.Key;
            }
        }

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
}
