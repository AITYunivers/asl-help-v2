using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

using AslHelp.Common.Exceptions;
using AslHelp.Core.IO;
using AslHelp.Core.LiveSplitInterop;
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
            string action = Actions.CurrentAction;
            if (action is "startup" or "exit" or "shutdown")
            {
                string msg = $"Attempted to access the memory manager in the '{action}' action.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            if (_memory is not null)
            {
                return _memory;
            }

            if (Game is Process game)
            {
                InitMemory(game);
            }

            return _memory;
        }
    }

    protected virtual void InitMemory(Process process)
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
                    _memory = new PipeMemoryManager(process, Logger, "asl-help-pipe", _timeout);

                    Debug.Info("    => Success.");

                    return;
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

        _memory = new WinApiMemoryManager(process, Logger);
        _pointers = new(_memory);
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

    protected virtual void DisposeMemory()
    {
        _memory?.Dispose();
        _memory = null;

        _game?.Dispose();
        _game = null;

        _pointers = null;
    }
}
