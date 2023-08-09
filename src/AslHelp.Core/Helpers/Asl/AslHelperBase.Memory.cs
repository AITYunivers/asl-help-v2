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

    protected abstract IMemoryManager InitializeExternalMemory(Process process, ILogger logger);
    protected abstract IMemoryManager InitializeInternalMemory(Process process, ILogger logger, NamedPipeClientStream pipe);

    protected IMemoryManager InitializeMemory(Process process)
    {
        Debug.Info("Initiating memory...");
        bool is64Bit = process.ProcessIs64Bit();

        if (_inject && TryInjectAslHelpNative(process, is64Bit, _pipeConnectionTimeout, out var pipe))
        {
            return InitializeInternalMemory(process, Logger, pipe);
        }
        else
        {
            Debug.Info("  => Using Win32 API for memory reading.");

            return InitializeExternalMemory(process, Logger);
        }
    }

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
    private static unsafe bool TryInjectAslHelpNative(Process process, bool is64Bit, int timeout, [NotNullWhen(true)] out NamedPipeClientStream? pipe)
    {
        Debug.Info("  => Attempting to inject...");

        nuint processHandle = (nuint)(nint)process.Handle;
        uint processId = (uint)process.Id;

        string arch = is64Bit ? "x64" : "x86";
        string res = $"AslHelp.Native.{arch}.dll";

        string path = Path.GetFullPath($"{arch}/{res}");

        if (!WinInteropWrapper.IsInjected(processHandle, processId, path, out Module? module))
        {
            // if (!is64Bit && !WinInteropWrapper.TryInjectDll(processHandle, "msvcrt"))
            // {
            //     pipe = null;

            //     Debug.Warn("    => Failure!");
            //     return false;
            // }

            if (!EmbeddedResource.TryInject(processHandle, res, arch)
                || !WinInteropWrapper.IsInjected(processHandle, processId, path, out module))
            {
                pipe = null;

                Debug.Warn("    => Failure!");
                return false;
            }
        }

        if (!WinInteropWrapper.TryCallEntryPoint(processHandle, module.Base, "AslHelp_Native_EntryPoint"u8))
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
            pipe.Connect(timeout);

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
