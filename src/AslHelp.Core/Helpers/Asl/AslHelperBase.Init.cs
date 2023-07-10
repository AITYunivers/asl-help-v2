using AslHelp.Common.Exceptions;
using AslHelp.Core.Helpers.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase : IAslHelper.Initialization
{
    private bool _generateCode;

    protected bool _initialized;
    protected bool _withInjection;
    protected int _pipeConnectionTimeout;

    protected abstract IAslHelper InitImpl();
    protected abstract void GenerateCode();

    public IAslHelper Init()
    {
        if (_initialized)
        {
            string msg = "asl-help is already initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Debug.Info("Initializing asl-help...");

        InitImpl();

        if (_generateCode)
        {
            Debug.Info("  => Generating code...");
            GenerateCode();
        }

        Debug.Info("  => Done.");

        _initialized = true;

        return this;
    }

    public IAslHelper.Initialization SetGameName(string gameName)
    {
        _gameName = gameName;
        return this;
    }

    public IAslHelper.Initialization DoCodeGeneration(bool generateCode = true)
    {
        if (_initialized)
        {
            string msg = "Code generation may only be enabled before initialization.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        _generateCode = generateCode;

        return this;
    }

    public IAslHelper.Initialization DoInjection(int pipeConnectionTimeout = 3000)
    {
        if (_initialized)
        {
            string msg = "Injection may only be enabled before initialization.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        ThrowHelper.ThrowIfLessThan(pipeConnectionTimeout, -1);

        _withInjection = true;
        _pipeConnectionTimeout = pipeConnectionTimeout;

        return this;
    }
}
