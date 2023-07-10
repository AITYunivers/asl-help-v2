using System;
using System.Diagnostics;
using System.IO;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.IO;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Memory.Native;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase
{
    public abstract IMemoryManager? Memory { get; protected set; }

    protected abstract IMemoryManager InitializePipeMemory(Process process, ILogger logger, string pipeName, int timeout);
    protected abstract IMemoryManager InitializeWinApiMemory(Process process, ILogger logger);

    protected IMemoryManager InitializeMemory(Process process)
    {
        Debug.Info("Initiating memory...");
        bool is64Bit = process.ProcessIs64Bit();

        if (_withInjection && is64Bit)
        {
            Debug.Info("  => Attempting to inject...");

            if (TryInjectAslCoreNative(process, is64Bit))
            {
                Debug.Info("    => Success.");
                Debug.Info("  => Connecting named pipe...");

                try
                {
                    IMemoryManager memory = InitializePipeMemory(process, Logger, "asl-help-pipe", _pipeConnectionTimeout);

                    Debug.Info("    => Success.");

                    return memory;
                }
                catch (TimeoutException)
                {
                    Debug.Warn("    => Timed out!");
                }
            }
            else
            {
                Debug.Warn("    => Failure!");
            }
        }

        Debug.Info("  => Using Win32 API for memory reading.");

        return InitializeWinApiMemory(process, Logger);
    }

    protected virtual void DisposeMemory()
    {
        Memory?.Dispose();
        Memory = null;
    }

    private static unsafe bool TryInjectAslCoreNative(Process process, bool is64Bit)
    {
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
                return false;
            }
        }

        return WinInteropWrapper.TryCallEntryPoint(processHandle, module.Base, "AslHelpPipe.EntryPoint"u8);
    }
}
