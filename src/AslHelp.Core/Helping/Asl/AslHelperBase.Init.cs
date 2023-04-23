using AslHelp.Core.Exceptions;
using AslHelp.Core.Helping.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;

namespace AslHelp.Core.Helping.Asl;

public abstract partial class AslHelperBase
{
    protected bool _generateCode;
    protected bool _withInjection;
    protected int _timeout;
    protected bool _isCompleted;

    public IAslHelper.InitStage Init
    {
        get
        {
            if (Actions.CurrentAction != "startup")
            {
                string msg = "asl-help may only be initialized in the 'startup' action.";
                ThrowHelper.Throw.InvalidOperation(msg);
            }

            return this;
        }
    }

    protected abstract void GenerateCode();

    IAslHelper.InitStage IAslHelper.InitStage.GenerateCode()
    {
        if (Actions.CurrentAction != "startup")
        {
            string msg = "Code may only be generated in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        _generateCode = true;

        return this;
    }

    IAslHelper.InitStage IAslHelper.InitStage.WithInjection()
    {
        if (Actions.CurrentAction != "startup")
        {
            string msg = "Injection may only be enabled in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        _withInjection = true;
        _timeout = 3000;

        return this;
    }

    IAslHelper.InitStage IAslHelper.InitStage.WithInjection(int pipeConnectionTimeout)
    {
        if (Actions.CurrentAction != "startup")
        {
            string msg = "Injection may only be enabled in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        _withInjection = true;
        _timeout = pipeConnectionTimeout;

        return this;
    }

    protected abstract void Complete();

    IAslHelper IAslHelper.InitStage.Complete()
    {
        if (_isCompleted)
        {
            string msg = "asl-help is already initialized.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        if (Actions.CurrentAction != "startup")
        {
            string msg = "asl-help may only be initialized in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        Debug.Info("Initializing asl-help...");

        AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        Complete();

        if (_generateCode)
        {
            Debug.Info("  => Generating code...");
            GenerateCode();
        }

        Debug.Info("  => Success. Don't forget to call `.Complete()`.");

        _isCompleted = true;

        return this;
    }

    protected static Assembly AssemblyResolve(object sender, ResolveEventArgs e)
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
