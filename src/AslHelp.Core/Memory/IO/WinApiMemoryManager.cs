using AslHelp.Core.Extensions;
using AslHelp.Core.IO.Logging;
using CommunityToolkit.HighPerformance.Buffers;
using LiveSplit.ComponentUtil;
using System.Text;
using ArrayPoolExtensions = AslHelp.Core.Extensions.ArrayPoolExtensions;

namespace AslHelp.Core.Memory.IO;

public sealed class WinApiMemoryManager : MemoryManagerBase
{
    private const byte AsciiNullChar = 0;
    private static ReadOnlySpan<byte> UnicodeNullChar => new byte[] { 0, 0 };

    public WinApiMemoryManager(Process process)
        : base(process, null) { }

    public WinApiMemoryManager(Process process, ILogger logger)
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
                result = default;
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

    public sealed override unsafe bool TryReadString(out string result, int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            result = null;
            return false;
        }

        if (stringType != ReadStringType.AutoDetect)
        {
            (bool isUnicode, byte charSize, Encoding encoding) = stringType.GetEncodingInformation();
            return TryReadString(out result, deref, isUnicode ? length * 2 : length, charSize, encoding);
        }
        else
        {
            return TryReadStringAuto(out result, deref, length, length * 2);
        }
    }

    private unsafe bool TryReadStringAuto(out string result, nint address, int utf8Length, int unicodeLength)
    {
        using ArrayPoolBufferWriter<byte> writer = new(unicodeLength);

        byte[] rented = null;
        Span<byte> buffer =
            unicodeLength <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPoolExtensions.Rent<byte>(unicodeLength));

        fixed (byte* pBuffer = buffer)
        {
            if (!Process.Read(address, pBuffer, unicodeLength))
            {
                ArrayPoolExtensions.Return(rented);

                result = null;
                return false;
            }

            int length;
            byte charSize;
            Encoding encoding;

            if (utf8Length >= 2 && pBuffer[1] == '\0')
            {
                length = unicodeLength;
                charSize = 2;
                encoding = Encoding.Unicode;
            }
            else
            {
                length = utf8Length;
                charSize = 1;
                encoding = Encoding.UTF8;
            }

            for (int i = 0; i < length; i += charSize)
            {
                if (pBuffer[i] == '\0')
                {
                    ArrayPoolExtensions.Return(rented);

                    result = encoding.GetString(pBuffer, i);
                    return true;
                }
            }

            ArrayPoolExtensions.Return(rented);

            result = encoding.GetString(pBuffer, length);
            return true;
        }
    }

    private unsafe bool TryReadString(out string result, nint address, int length, byte charSize, Encoding encoding)
    {
        byte[] rented = null;
        Span<byte> buffer =
            length <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPoolExtensions.Rent<byte>(length));

        fixed (byte* pBuffer = buffer)
        {
            if (!Process.Read(address, pBuffer, length))
            {
                ArrayPoolExtensions.Return(rented);

                result = null;
                return false;
            }

            for (int i = 0; i < length; i += charSize)
            {
                if (pBuffer[i] == '\0')
                {
                    ArrayPoolExtensions.Return(rented);

                    result = encoding.GetString(pBuffer, i);
                    return true;
                }
            }

            ArrayPoolExtensions.Return(rented);

            result = encoding.GetString(pBuffer, length);
            return true;
        }
    }

    public override unsafe bool TryReadSizedString(out string result, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            result = null;
            return false;
        }

        (_, byte charSize, Encoding encoding) = stringType.GetEncodingInformation();

        int length = Read<int>(deref - 0x4) * charSize;

        byte[] rented = null;
        Span<byte> chars =
            length <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPoolExtensions.Rent<byte>(length));

        fixed (byte* pChars = chars)
        {
            if (!Process.Read(deref, pChars, length))
            {
                ArrayPoolExtensions.Return(rented);

                result = null;
                return false;
            }

            result = encoding.GetString(pChars, length);

            ArrayPoolExtensions.Return(rented);

            return true;
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
