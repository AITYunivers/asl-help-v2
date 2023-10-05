using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Collections;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Native;
using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public abstract partial class MemoryManagerBase : IMemoryManager
{
    private readonly ILogger? _logger;

    protected readonly nuint _processHandle;
    protected bool _isDisposed;

    public MemoryManagerBase(Process process)
    {
        ThrowHelper.ThrowIfNull(process);
        if (process.HasExited)
        {
            ThrowHelper.ThrowInvalidOperationException("Cannot interact with the memory of an exited process.");
        }

        _logger = null;
        _processHandle = (nuint)(nint)process.Handle;

        Process = process;
        Is64Bit = WinInteropWrapper.ProcessIs64Bit(_processHandle);
        PointerSize = (byte)(Is64Bit ? 0x8 : 0x4);
        Modules = new(process);
        MainModule = Modules.First();
    }

    public MemoryManagerBase(Process process, ILogger logger)
    {
        ThrowHelper.ThrowIfNull(process);
        if (process.HasExited)
        {
            ThrowHelper.ThrowInvalidOperationException("Cannot interact with the memory of an exited process.");
        }

        _logger = logger;
        _processHandle = (nuint)(nint)process.Handle;

        Process = process;
        Is64Bit = WinInteropWrapper.ProcessIs64Bit(_processHandle);
        PointerSize = (byte)(Is64Bit ? 0x8 : 0x4);
        Modules = new(process);
        MainModule = Modules.First();
    }

    public Process Process { get; }
    public bool Is64Bit { get; }
    public byte PointerSize { get; }

    public Module MainModule { get; }
    public ModuleCache Modules { get; }

    public IEnumerable<MemoryPage> Pages(bool allPages)
    {
        return WinInteropWrapper.EnumerateMemoryPages(_processHandle, Is64Bit, allPages);
    }

    public uint Tick { get; private set; }

    public virtual void Update()
    {
        Tick++;
    }

    public nuint ReadRelative(nuint address)
    {
        if (Is64Bit)
        {
            return address + 0x4 + Read<uint>(address);
        }
        else
        {
            return Read<uint>(address);
        }
    }

    public bool TryReadRelative(nuint address, out nuint result)
    {
        if (Is64Bit)
        {
            bool success = TryRead(out uint offset, address);
            result = address + 0x4 + offset;

            return success;
        }
        else
        {
            return TryRead(out result, address);
        }
    }

    public void Log(object output)
    {
        _logger?.Log(output);
    }

    public virtual void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        Process.Dispose();

        _isDisposed = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static bool IsNativeInt<T>()
    {
        return typeof(T) == typeof(nint) || typeof(T) == typeof(nuint);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected unsafe uint GetNativeSizeOf<T>(int count = 1)
        where T : unmanaged
    {
        return (uint)(IsNativeInt<T>() ? PointerSize : sizeof(T)) * (uint)count;
    }
}
