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
            string msg = "[Deref] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = "[Deref] Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        nuint result = baseAddress, handle = _processHandle;
        uint ptrSize = PtrSize;

        for (int i = 0; i < offsets.Length; i++)
        {
            if (!WinInteropWrapper.ReadMemory(handle, result, &result, ptrSize) || result == 0)
            {
                string msg = "[Deref] Failed to dereference pointer.";
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
            string msg = "[TryDeref] Cannot interact with the memory of an exited process.";
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

    public override unsafe T Read<T>(nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[Read<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        nuint deref = Deref(baseAddress, offsets), handle = _processHandle;
        uint size = typeof(T) == typeof(nint) || typeof(T) == typeof(nuint) ? PtrSize : (uint)sizeof(T);

        T result;
        if (!WinInteropWrapper.ReadMemory(handle, deref, &result, size))
        {
            string msg = $"[Read<{typeof(T).Name}>] Failed to read value.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return result;
    }

    public override unsafe bool TryRead<T>(out T result, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[TryRead<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        nuint deref = Deref(baseAddress, offsets), handle = _processHandle;
        uint size = typeof(T) == typeof(nint) || typeof(T) == typeof(nuint) ? PtrSize : (uint)sizeof(T);

        fixed (T* pResult = &result)
        {
            if (!WinInteropWrapper.ReadMemory(handle, deref, pResult, size))
            {
                result = default;
                return false;
            }

            return true;
        }
    }

    public override unsafe void ReadSpan<T>(Span<T> buffer, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[ReadSpan<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (buffer.IsEmpty)
        {
            return;
        }

        nuint deref = Deref(baseAddress, offsets), handle = _processHandle;
        uint size = GetNativeSizeOf<T>() * (uint)buffer.Length;

        fixed (T* pBuffer = buffer)
        {
            if (!WinInteropWrapper.ReadMemory(handle, deref, pBuffer, size))
            {
                string msg = $"[ReadSpan<{typeof(T).Name}>] Failed to read values.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }
        }
    }

    public override unsafe bool TryReadSpan<T>(Span<T> buffer, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[TryReadSpan<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (buffer.IsEmpty)
        {
            return true;
        }

        nuint deref = Deref(baseAddress, offsets), handle = _processHandle;
        uint size = GetNativeSizeOf<T>() * (uint)buffer.Length;

        fixed (T* pBuffer = buffer)
        {
            if (!WinInteropWrapper.ReadMemory(handle, deref, pBuffer, size))
            {
                return false;
            }

            return true;
        }
    }

    public override unsafe void Write<T>(T value, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[Write<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        nuint deref = Deref(baseAddress, offsets), handle = _processHandle;
        uint size = GetNativeSizeOf<T>();

        if (!WinInteropWrapper.ReadMemory(handle, deref, &value, size))
        {
            string msg = $"[ReadSpan<{typeof(T).Name}>] Failed to write value.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }
    }

    public override unsafe bool TryWrite<T>(T value, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[Write<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        nuint deref = Deref(baseAddress, offsets), handle = _processHandle;
        uint size = GetNativeSizeOf<T>();

        return WinInteropWrapper.ReadMemory(handle, deref, &value, size);
    }

    public override unsafe void WriteSpan<T>(ReadOnlySpan<T> values, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[WriteSpan<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (values.IsEmpty)
        {
            return;
        }

        nuint deref = Deref(baseAddress, offsets), handle = _processHandle;
        uint size = GetNativeSizeOf<T>() * (uint)values.Length;

        fixed (T* pValues = values)
        {
            if (!WinInteropWrapper.ReadMemory(handle, deref, pValues, size))
            {
                string msg = $"[ReadSpan<{typeof(T).Name}>] Failed to write values.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }
        }
    }

    public override unsafe bool TryWriteSpan<T>(ReadOnlySpan<T> values, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[WriteSpan<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (values.IsEmpty)
        {
            return true;
        }

        nuint deref = Deref(baseAddress, offsets), handle = _processHandle;
        uint size = GetNativeSizeOf<T>() * (uint)values.Length;

        fixed (T* pValues = values)
        {
            return WinInteropWrapper.ReadMemory(handle, deref, pValues, size);
        }
    }
}
