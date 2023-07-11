using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipes;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.IO;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Memory.Native;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase
{
    public abstract IMemoryManager? Memory { get; protected set; }

    /// <summary>
    ///     Initializes an instance of an <see cref="IMemoryManager"/> implementation
    ///     with the specified <see cref="Process"/> and <see cref="ILogger"/>.
    /// </summary>
    /// <param name="process">The target <see cref="Process"/> whose memory should be managed.</param>
    /// <param name="logger">The <see cref="ILogger"/> to use for logging.</param>
    /// <returns>
    ///     The created <see cref="IMemoryManager"/> instance.
    /// </returns>
    protected abstract IMemoryManager InitializeMemory(Process process, ILogger logger);

    /// <summary>
    ///     Disposes of any resources pertaining to managing memory.
    /// </summary>
    protected virtual void DisposeMemory()
    {
        Memory?.Dispose();
        Memory = null;
    }

    /// <summary>
    ///     Attempts to inject the AslHelp.Native library into the specified <see cref="Process"/>.
    /// </summary>
    /// <param name="process">The target <see cref="Process"/> to inject into.</param>
    /// <param name="is64Bit">Specifies whether the target <see cref="Process"/> is 64-bit.</param>
    /// <param name="pipe">
    ///     Contains the <see cref="NamedPipeClientStream"/> instance
    ///     used to communicate with the injected library, if the injection was successful;
    ///     otherwise, <see langword="null"/>.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the injection was successful;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    protected static unsafe bool TryInjectAslHelpNative(Process process, bool is64Bit, [NotNullWhen(true)] out NamedPipeClientStream? pipe)
    {
        Debug.Info("  => Attempting to inject...");

        nuint processHandle = (nuint)(nint)process.Handle;
        uint processId = (uint)process.Id;

        string arch = is64Bit ? "x64" : "x86";
        string res = $"AslHelp.Native.{arch}.dll";

        string path = Path.GetFullPath($"{arch}/{res}");

        if (!WinInteropWrapper.IsInjected(processHandle, processId, path, out Module? module))
        {
            if (!EmbeddedResource.TryInject(processHandle, res, arch)
                || !WinInteropWrapper.IsInjected(processHandle, processId, path, out module))
            {
                pipe = null;

                Debug.Warn("    => Failure!");
                return false;
            }
        }

        if (!WinInteropWrapper.TryCallEntryPoint(processHandle, module.Base, "AslHelpPipe.EntryPoint"u8))
        {
            pipe = null;

            Debug.Warn("    => Failure!");
            return false;
        }

        Debug.Info("    => Success.");
        Debug.Info("  => Connecting named pipe...");

        try
        {
            pipe = new("asl-help-pipe");
            pipe.Connect(3000);

            Debug.Info("    => Success.");
            return true;
        }
        catch (TimeoutException)
        {
            pipe = null;

            Debug.Warn("    => Timed out!");
            return false;
        }
    }
}
