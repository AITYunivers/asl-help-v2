using AslHelp.Common.Exceptions;
using AslHelp.Core.Helping.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;

namespace AslHelp.Core.Helping.Asl;

public abstract partial class AslHelperBase
{
    private bool _generateCode;
    private bool _isCompleted;

    protected bool _withInjection;
    protected int _timeout;

    public IAslHelper.InitStage Init
    {
        get
        {
            if (Actions.CurrentAction != "startup")
            {
                string msg = "asl-help may only be initialized in the 'startup' action.";
                ThrowHelper.ThrowInvalidOperationException(msg);
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
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        _generateCode = true;

        return this;
    }

    IAslHelper.InitStage IAslHelper.InitStage.WithInjection()
    {
        if (Actions.CurrentAction != "startup")
        {
            string msg = "Injection may only be enabled in the 'startup' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
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
            ThrowHelper.ThrowInvalidOperationException(msg);
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
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (Actions.CurrentAction != "startup")
        {
            string msg = "asl-help may only be initialized in the 'startup' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Debug.Info("Initializing asl-help...");

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
}
