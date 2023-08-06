using AslHelp.Common.Exceptions;
using AslHelp.Core.Helpers.Asl.Contracts;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase : IAslHelper.Initialization
{
    /// <summary>
    ///     Specifies whether <see cref="Init"/> has been called successfully.
    /// </summary>
    protected bool _initialized;

    private bool _inject;
    private int _pipeConnectionTimeout;

    /// <summary>
    ///     This method is called from <see cref="Init"/> after some checks are performed.
    /// </summary>
    protected abstract void InitImpl();

    /// <summary>
    ///     This method is called from <see cref="Init"/> when <see cref="DoCodeGeneration"/> was called with <see langword="true"/>.
    /// </summary>
    protected abstract void GenerateCode();

    public IAslHelper Init(string? gameName = null, bool generateCode = false, bool inject = false, int timeout = 3000)
    {
        if (_initialized)
        {
            string msg = "asl-help is already initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        ThrowHelper.ThrowIfLessThan(timeout, 0);

        _gameName = gameName;
        _inject = inject;
        _pipeConnectionTimeout = timeout;

        Debug.Info("Initializing asl-help...");

        InitImpl();

        if (generateCode)
        {
            Debug.Info("  => Generating code...");
            GenerateCode();
        }

        Debug.Info("  => Done.");

        _initialized = true;

        return this;
    }
}
