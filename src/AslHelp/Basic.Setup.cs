using System.Reflection;
using AslHelp.Core.Exceptions;
using AslHelp.Core.Helping;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.LiveSplitInterop;

public partial class Basic
    : IAslHelperInitStage,
    IAslHelperSetupStage
{
    private bool _isInitialized;
    private bool _generatedCode;
    private bool _withInjection;
    private bool _isCompleted;

    public IAslHelperSetupStage Init()
    {
        if (_isInitialized)
        {
            string msg = "asl-help is already initialized.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        if (Methods.CurrentMethod != "startup")
        {
            string msg = "asl-help may only be initialized in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        _logger = new(new DebugLogger());

        Debug.Info("Initializing asl-help...");

        AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        AslHelp.Core.LiveSplitInterop.Timer.Init();
        Script.Init();

        Debug.Info("  => Success. Don't forget to call `.Complete()`.");

        _isInitialized = true;

        return this;
    }

    public virtual IAslHelperSetupStage GenerateCode()
    {
        if (_generatedCode)
        {
            string msg = "asl-help already generated code.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        if (Methods.CurrentMethod != "startup")
        {
            string msg = "Code may only be generated in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        Debug.Info("  => Generating code...");

        Script.Vars["Log"] = (Action<object>)(output => _logger.Log(output));
        Debug.Info("    => Created the Action<object> `vars.Log`.");

        Methods.exit.Prepend($"vars.AslHelp.{nameof(Exit)}();");
        Methods.shutdown.Prepend($"vars.AslHelp.{nameof(Shutdown)}();");

        _generatedCode = true;

        return this;
    }

    public virtual IAslHelperSetupStage WithInjection()
    {
        if (_withInjection)
        {
            string msg = "WithInjection is already set to true.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        if (Methods.CurrentMethod != "startup")
        {
            string msg = "Injection may only be enabled in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        _withInjection = true;

        return this;
    }

    public IAslHelper Complete()
    {
        if (!_isInitialized)
        {
            string msg = "asl-help was not initialized.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        if (_isCompleted)
        {
            string msg = "asl-help initialization is already completed.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        if (Methods.CurrentMethod != "startup")
        {
            string msg = "asl-help may only be initialized in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        _isCompleted = true;

        return this;
    }

    protected void EnsureInitialized()
    {
        if (!_isInitialized)
        {
            Debug.Error("asl-help is uninitialized. Did you forget to call `.Init()` in the 'startup' action?");

            string msg = "Cannot operate on an uninitialized helper.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        if (!_isCompleted)
        {
            Debug.Error("asl-help is incomplete. Did you forget to call `.Complete()` in the 'startup' action?");

            string msg = "Cannot operate on an incomplete helper.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }
    }

    private Assembly AssemblyResolve(object sender, ResolveEventArgs e)
    {
        string name = e.Name;
        int i = name.IndexOf(',');
        if (i == -1)
        {
            ThrowHelper.Throw.Argument(nameof(e.Name), "Assembly name was in an unexpected format.");
        }

        string file = $"Components/{name[..i]}.dll";

        return File.Exists(file) ? Assembly.LoadFrom(file) : null;
    }
}
