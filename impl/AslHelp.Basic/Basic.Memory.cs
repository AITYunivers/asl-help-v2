using System;
using System.Diagnostics;
using System.IO;

using AslHelp.Common.Exceptions;
using AslHelp.Core.LiveSplitInterop;
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

    private static bool TryInjectAslCoreNative(Process process, bool is64Bit)
    {
        string arch = is64Bit ? "x64" : "x86";
        string res = $"AslHelp.Core.Native.{arch}.dll";

        string path = Path.GetFullPath($"{arch}/{res}");

        return (process.IsInjected(path, out nuint moduleBase)
            || process.TryInjectDll(path, "AslHelp.Core.Native.DllMain"))
            && WinInteropWrapper.TryCallEntryPoint((nuint)(nint)process.Handle, moduleBase, "AslHelp.DllMain");
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
