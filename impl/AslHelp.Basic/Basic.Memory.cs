using System;
using System.Diagnostics;
using System.IO;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.IO;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Memory.Native;

using Debug = AslHelp.Core.Diagnostics.Debug;

public partial class Basic
{
    private IMemoryManager? _memory;
    public IMemoryManager? Memory
    {
        get
        {
            if (_memory is not null)
            {
                return _memory;
            }

            if (Game is Process game)
            {
                _memory = InitMemory(game);
                _pointers = new(_memory);
            }

            return _memory;
        }
    }

    private IMemoryManager InitMemory(Process process)
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
                    PipeMemoryManager memory = InitPipeMemory(process, Logger, "asl-help-pipe", _timeout);

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

        return InitWinApiMemory(process, Logger);
    }

    protected virtual WinApiMemoryManager InitWinApiMemory(Process process, ILogger logger)
    {
        return new(process, logger);
    }

    protected virtual PipeMemoryManager InitPipeMemory(Process process, ILogger logger, string pipeName, int timeout)
    {
        return new(process, logger, pipeName, timeout);
    }

    protected virtual void DisposeMemory()
    {
        _memory?.Dispose();
        _memory = null;

        _game?.Dispose();
        _game = null;

        _pointers = null;
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
