using AslHelp.Common.Exceptions;
using AslHelp.Core.Helpers.Asl.Contracts;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase : IAslHelper.Initialization
{
    private bool _generateCode;

    protected bool _initialized;

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
}
