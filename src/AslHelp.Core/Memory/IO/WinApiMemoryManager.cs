using AslHelp.Core.Exceptions;
using AslHelp.Core.IO.Logging;

namespace AslHelp.Core.Memory.IO;

public sealed class WinApiMemoryManager : MemoryManagerBase
{
    public WinApiMemoryManager(Process process)
        : this(process, null) { }

    public WinApiMemoryManager(Process process, ILogger logger)
        : base(process, logger) { }

    public override unsafe bool TryDeref(out nint result, nint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            ThrowHelper.Throw.InvalidOperation("Cannot interact with the memory of an exited process.");
        }

        if (baseAddress == 0)
        {
            result = default;
            return false;
        }

        result = baseAddress;

        if (offsets.Length == 0)
        {
            return true;
        }

        nint handle = _processHandle;
        int size = PtrSize;

        fixed (nint* pResult = &result)
        {
            for (int i = 0; i < offsets.Length; i++)
            {
                if (!Native.Read(handle, result, pResult, size) || result == default)
                {
                    result = default;
                    return false;
                }

                result += offsets[i];
            }

            return true;
        }
    }

    public override unsafe bool TryRead<T>(out T result, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            result = default;
            return false;
        }

        fixed (T* pResult = &result)
        {
            if (!Native.Read(_processHandle, deref, pResult, Native.GetTypeSize<T>(PtrSize)))
            {
                result = default;
                return false;
            }

            return !Native.IsNativeInt<T>() || !result.Equals(default(T));
        }
    }

    public override unsafe bool TryReadSpan<T>(Span<T> buffer, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            return false;
        }

        if (!Is64Bit && Native.IsNativeInt<T>())
        {
            Span<uint> buf32 = MemoryMarshal.Cast<T, uint>(buffer);
            Span<ulong> buf64 = MemoryMarshal.Cast<T, ulong>(buffer);

            int length = buf64.Length;
            if (!TryReadSpan(buf32[length..], deref))
            {
                return false;
            }

            for (int i = 0; i < length; i++)
            {
                buf64[i] = buf32[length + i];
            }

            return true;
        }

        fixed (T* pBuffer = buffer)
        {
            return Native.Read(_processHandle, deref, pBuffer, Native.GetTypeSize<T>(PtrSize) * buffer.Length);
        }
    }

    public override unsafe bool Write<T>(T value, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            return false;
        }

        return Native.Write(_processHandle, deref, &value, Native.GetTypeSize<T>(PtrSize));
    }

    public override unsafe bool WriteSpan<T>(ReadOnlySpan<T> values, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            return false;
        }

        if (!Is64Bit && Native.IsNativeInt<T>())
        {
            ReadOnlySpan<uint> v32 = MemoryMarshal.Cast<T, uint>(values);
            return WriteSpan(v32, deref);
        }

        fixed (T* pValues = values)
        {
            return Native.Write(_processHandle, baseAddress, pValues, Native.GetTypeSize<T>(PtrSize) * values.Length);
        }
    }
}
