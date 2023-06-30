using System;
using System.IO;
using System.Reflection;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Diagnostics;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.LiveSplitInterop;

public partial class Basic
{
    private bool _isInitialized;
    private bool _generateCode;

    private bool _withInjection;
    private int _timeout;

    public Basic WithCodeGeneration(bool generateCode = true)
    {
        if (Actions.CurrentAction != "startup")
        {
            string msg = "Code may only be generated in the 'startup' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        _generateCode = generateCode;

        return this;
    }

    protected virtual void GenerateCode()
    {
        Script.Vars["Log"] = (Action<object>)(output => Logger.Log($"[{GameName}] {output}"));
        Debug.Info("    => Created the Action<object> `vars.Log`.");

        Actions.exit.Prepend($"vars.AslHelp.{nameof(Exit)}();");
        Actions.shutdown.Prepend($"vars.AslHelp.{nameof(Shutdown)}();");
    }

    public Basic WithInjection(int pipeConnectionTimeout = 3000)
    {
        if (Actions.CurrentAction != "startup")
        {
            string msg = "Injection may only be enabled in the 'startup' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        ThrowHelper.ThrowIfLessThan(pipeConnectionTimeout, -1);

        _withInjection = true;
        _timeout = pipeConnectionTimeout;

        return this;
    }

    public Basic Init()
    {
        if (_isInitialized)
        {
            string msg = "asl-help is already initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        _isInitialized = true;

        if (Actions.CurrentAction != "startup")
        {
            string msg = "asl-help may only be initialized in the 'startup' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Debug.Info("Initializing asl-help...");

        AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;

        Logger.Add(new DebugLogger());

        AslHelp.Core.LiveSplitInterop.Timer.Init();
        Script.Init();

        if (_generateCode)
        {
            Debug.Info("  => Generating code...");
            GenerateCode();
        }

        Debug.Info("  => Done.");

        return this;
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
