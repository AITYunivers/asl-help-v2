using System;
using System.IO;
using System.Reflection;
using AslHelp.Common.Exceptions;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.LiveSplitInterop;

public partial class Basic
{
    protected override void GenerateCode()
    {
        Script.Vars["Log"] = (Action<object>)(output => _logger.Log($"[{GameName}] {output}"));
        Debug.Info("    => Created the Action<object> `vars.Log`.");

        Actions.exit.Prepend($"vars.AslHelp.{nameof(Exit)}();");
        Actions.shutdown.Prepend($"vars.AslHelp.{nameof(Shutdown)}();");
    }

    protected override void Complete()
    {
        AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;

        _logger.Add(new DebugLogger());

        AslHelp.Core.LiveSplitInterop.Timer.Init();
        Script.Init();
    }

    private static Assembly AssemblyResolve(object sender, ResolveEventArgs e)
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
