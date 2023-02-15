using AslHelp.Core.IO.Logging;
using LiveSplit.ComponentUtil;
using System.Text;

namespace AslHelp.Core.Memory.IO;

public sealed class WinApiMemoryManager : MemoryManagerBase
{
    public WinApiMemoryManager(Process process, LoggerBase logger)
        : base(process, logger) { }

    public sealed override unsafe bool TryDeref(out nint result, nint baseAddress, params int[] offsets)
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

    public sealed override unsafe bool TryRead<T>(out T result, nint baseAddress, params int[] offsets)
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

            return !Native.IsPointer<T>() || !result.Equals(default(T));
        }
    }

    public sealed override unsafe bool TryReadSpan<T>(Span<T> buffer, nint baseAddress, params int[] offsets)
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
            return Process.Read(deref, pBuffer, Native.GetTypeSize<T>(Is64Bit) * buffer.Length);
        }
    }

    public sealed override bool TryReadString(out string result, int length, ReadStringType stringType, bool sized, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            result = null;
            return false;
        }

        Encoding encoding;
        bool isUnicode;
        byte charSize;

        setEncoding(stringType == ReadStringType.UTF16);

        result = default;
        return true;

        void setEncoding(bool unicode)
        {
            encoding = unicode ? Encoding.Unicode : (stringType == ReadStringType.ASCII ? Encoding.ASCII : Encoding.UTF8);
            isUnicode = unicode;
            charSize = (byte)(unicode ? 2 : 1);
        }
    }

    public sealed override unsafe bool Write<T>(T value, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            return false;
        }

        return Process.Write(deref, &value, Native.GetTypeSize<T>(Is64Bit));
    }

    public sealed override unsafe bool WriteSpan<T>(ReadOnlySpan<T> values, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            return false;
        }

        if (!Is64Bit && Native.IsPointer<T>())
        {
            ReadOnlySpan<uint> v32 = MemoryMarshal.Cast<T, uint>(values);
            return WriteSpan(v32, deref);
        }

        fixed (T* pValues = values)
        {
            return Process.Write(baseAddress, pValues, Native.GetTypeSize<T>(Is64Bit) * values.Length);
        }
    }
}
