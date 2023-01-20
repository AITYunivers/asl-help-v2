﻿using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.IO;

public class Win32MemoryManager : MemoryManagerBase
{
    public override unsafe bool TryDeref(out nint result, nint baseAddress, params int[] offsets)
    {
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

        fixed (nint* pResult = &result)
        {
            for (int i = 0; i < offsets.Length; i++)
            {
                if (!Process.Read(result, pResult, Is64Bit ? 0x8 : 0x4) || result == default)
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
            if (!Process.Read(deref, pResult, Native.GetTypeSize<T>(Is64Bit)))
            {
                return false;
            }

            return !Native.IsPointer<T>() || !result.Equals(default);
        }
    }

    public override unsafe bool TryReadSpan<T>(Span<T> buffer, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            return false;
        }

        if (!Is64Bit && Native.IsPointer<T>())
        {
            Span<uint> buf32 = MemoryMarshal.Cast<T, uint>(buffer);
            Span<ulong> buf64 = MemoryMarshal.Cast<T, ulong>(buffer);

            int length = buf64.Length;
            if (!TryReadSpan<uint>(buf32[length..], deref))
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
            return Process.Read(deref, pBuffer, Native.GetTypeSize<T>(Is64Bit) * buffer.Length);
        }
    }

    public override bool TryReadString(out string result, int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public override unsafe bool Write<T>(T value, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            return false;
        }

        return Process.Write(deref, &value, Native.GetTypeSize<T>(Is64Bit));
    }

    public override unsafe bool WriteSpan<T>(ReadOnlySpan<T> values, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            return false;
        }

        if (!Is64Bit && Native.IsPointer<T>())
        {
            ReadOnlySpan<uint> v32 = MemoryMarshal.Cast<T, uint>(values);
            return WriteSpan<uint>(v32, deref);
        }

        fixed (T* pValues = values)
        {
            return Process.Write(baseAddress, pValues, Native.GetTypeSize<T>(Is64Bit) * values.Length);
        }
    }
}
