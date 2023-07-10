using System;
using System.Diagnostics;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Native;

namespace AslHelp.Core.Memory.Ipc;

public class WinApiMemoryManager : MemoryManagerBase
{
    public WinApiMemoryManager(Process process)
        : base(process) { }

    public WinApiMemoryManager(Process process, ILogger logger)
        : base(process, logger) { }

    public override unsafe nuint Deref(nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = "Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        nuint result = baseAddress, handle = _processHandle;
        uint ptrSize = PtrSize;

        for (int i = 0; i < offsets.Length; i++)
        {
            if (!WinInteropWrapper.ReadMemory(handle, result, &result, ptrSize) || result == 0)
            {
                string msg = "Failed to dereference pointer.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            result += (uint)offsets[i];
        }

        return result;
    }

    public override unsafe bool TryDeref(out nuint result, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            result = default;
            return false;
        }

        result = baseAddress;

        nuint handle = _processHandle;
        uint ptrSize = PtrSize;

        fixed (nuint* pResult = &result)
        {
            for (int i = 0; i < offsets.Length; i++)
            {
                if (!WinInteropWrapper.ReadMemory(handle, result, pResult, ptrSize) || result == 0)
                {
                    result = default;
                    return false;
                }

                result += (uint)offsets[i];
            }

            return true;
        }
    }

    protected override unsafe void Read<T>(T* buffer, uint length, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        nuint deref = Deref(baseAddress, offsets), handle = _processHandle;
        if (!WinInteropWrapper.ReadMemory(handle, deref, buffer, length))
        {
            string msg = "Failed to read value.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }
    }

    protected override unsafe bool TryRead<T>(T* buffer, uint length, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        nuint deref = Deref(baseAddress, offsets), handle = _processHandle;
        return WinInteropWrapper.ReadMemory(handle, deref, buffer, length);
    }

    protected override unsafe void Write<T>(T* data, uint length, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        nuint deref = Deref(baseAddress, offsets), handle = _processHandle;
        if (!WinInteropWrapper.ReadMemory(handle, deref, data, length))
        {
            string msg = "Failed to write value.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }
    }

    protected override unsafe bool TryWrite<T>(T* data, uint length, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        nuint deref = Deref(baseAddress, offsets), handle = _processHandle;
        return WinInteropWrapper.ReadMemory(handle, deref, data, length);
    }
}
