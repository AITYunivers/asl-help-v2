using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;
using AslHelp.Common.Memory;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    // ReadString

    public string ReadString(int length, uint baseOffset, params int[] offsets)
    {
        return ReadString(length, ReadStringType.AutoDetect, baseOffset, offsets);
    }

    public string ReadString(int length, ReadStringType stringType, uint baseOffset, params int[] offsets)
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = "MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ReadString(length, stringType, module, baseOffset, offsets);
    }

    public string ReadString(int length, string moduleName, uint baseOffset, params int[] offsets)
    {
        return ReadString(length, ReadStringType.AutoDetect, moduleName, baseOffset, offsets);
    }

    public string ReadString(int length, ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ReadString(length, stringType, module, baseOffset, offsets);
    }

    public string ReadString(int length, Module module, uint baseOffset, params int[] offsets)
    {
        return ReadString(length, ReadStringType.AutoDetect, module, baseOffset, offsets);
    }

    public string ReadString(int length, ReadStringType stringType, Module module, uint baseOffset, params int[] offsets)
    {
        return ReadString(length, stringType, module.Base + baseOffset, offsets);
    }

    public string ReadString(int length, nuint baseAddress, params int[] offsets)
    {
        return ReadString(length, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public string ReadString(int length, ReadStringType stringType, nuint baseAddress, params int[] offsets)
    {
        ThrowHelper.ThrowIfLessThan(length, 0);

        if (length == 0)
        {
            return "";
        }

        if (stringType == ReadStringType.AutoDetect)
        {
            return InternalReadAutoString(length, baseAddress, offsets);
        }
        else if (stringType == ReadStringType.UTF16)
        {
            return InternalReadWideString(length, baseAddress, offsets);
        }
        else
        {
            return InternalReadString(length, baseAddress, offsets);
        }
    }

    private unsafe string InternalReadString(int length, nuint baseAddress, int[] offsets)
    {
        sbyte[]? rented = null;
        Span<sbyte> buffer =
            length <= 1024
            ? stackalloc sbyte[1024]
            : (rented = ArrayPool<sbyte>.Shared.Rent(length));

        ReadSpan(buffer, baseAddress, offsets);

        fixed (sbyte* pBuffer = buffer[..length])
        {
            string result = new(pBuffer);
            ArrayPool<sbyte>.Shared.ReturnIfNotNull(rented);

            return result;
        }
    }

    private unsafe string InternalReadWideString(int length, nuint baseAddress, int[] offsets)
    {
        length *= 2;

        char[]? rented = null;
        Span<char> buffer =
            length <= 512
            ? stackalloc char[512]
            : (rented = ArrayPool<char>.Shared.Rent(length));

        ReadSpan(buffer, baseAddress, offsets);

        fixed (char* pBuffer = buffer[..length])
        {
            string result = new(pBuffer);
            ArrayPool<char>.Shared.ReturnIfNotNull(rented);

            return result;
        }
    }

    private unsafe string InternalReadAutoString(int length, nuint baseAddress, int[] offsets)
    {
        // Assume unicode for the worst-case scenario and just allocate length * 2.
        byte[]? rented = null;
        Span<byte> buffer =
            length * 2 <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPool<byte>.Shared.Rent(length * 2));

        ReadSpan(buffer, baseAddress, offsets);

        fixed (byte* pBuffer = buffer[..length])
        {
            // String ctor stops at the first null terminator.
            string result =
                length >= 2 && pBuffer[1] == '\0'
                ? new((char*)pBuffer)
                : new((sbyte*)pBuffer);

            ArrayPool<byte>.Shared.ReturnIfNotNull(rented);

            return result;
        }
    }

    // TryReadString

    public bool TryReadString([NotNullWhen(true)] out string? result, int length, uint baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, baseOffset, offsets);
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, uint baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, stringType, baseOffset, offsets);
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, int length, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, moduleName, baseOffset, offsets);
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets)
    {
        if (moduleName is null)
        {
            result = default;
            return false;
        }

        return TryReadString(out result, length, stringType, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, int length, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, module, baseOffset, offsets);
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            result = default;
            return false;
        }

        return TryReadString(out result, length, stringType, module.Base + baseOffset, offsets);
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, int length, nuint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, nuint baseAddress, params int[] offsets)
    {
        ThrowHelper.ThrowIfLessThan(length, 0);

        if (length == 0)
        {
            result = "";
            return true;
        }

        if (stringType == ReadStringType.AutoDetect)
        {
            return InternalTryReadAutoString(out result, length, baseAddress, offsets);
        }
        else if (stringType == ReadStringType.UTF16)
        {
            return InternalTryReadWideString(out result, length, baseAddress, offsets);
        }
        else
        {
            return InternalTryReadString(out result, length, baseAddress, offsets);
        }
    }

    private unsafe bool InternalTryReadString(out string? result, int length, nuint baseAddress, int[] offsets)
    {
        sbyte[]? rented = null;
        Span<sbyte> buffer =
            length <= 1024
            ? stackalloc sbyte[1024]
            : (rented = ArrayPool<sbyte>.Shared.Rent(length));

        if (!TryReadSpan(buffer, baseAddress, offsets))
        {
            ArrayPool<sbyte>.Shared.ReturnIfNotNull(rented);

            result = default;
            return false;
        }

        fixed (sbyte* pBuffer = buffer[..length])
        {
            result = new(pBuffer);
            ArrayPool<sbyte>.Shared.ReturnIfNotNull(rented);

            return true;
        }
    }

    private unsafe bool InternalTryReadWideString(out string? result, int length, nuint baseAddress, int[] offsets)
    {
        length *= 2;

        char[]? rented = null;
        Span<char> buffer =
            length <= 512
            ? stackalloc char[512]
            : (rented = ArrayPool<char>.Shared.Rent(length));

        if (!TryReadSpan(buffer, baseAddress, offsets))
        {
            ArrayPool<char>.Shared.ReturnIfNotNull(rented);

            result = default;
            return false;
        }

        fixed (char* pBuffer = buffer[..length])
        {
            result = new(pBuffer);
            ArrayPool<char>.Shared.ReturnIfNotNull(rented);

            return true;
        }
    }

    private unsafe bool InternalTryReadAutoString(out string? result, int length, nuint baseAddress, int[] offsets)
    {
        // Assume unicode for the worst-case scenario and just allocate length * 2.
        byte[]? rented = null;
        Span<byte> buffer =
            length * 2 <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPool<byte>.Shared.Rent(length * 2));

        if (!TryReadSpan(buffer, baseAddress, offsets))
        {
            ArrayPool<byte>.Shared.ReturnIfNotNull(rented);

            result = default;
            return false;
        }

        fixed (byte* pBuffer = buffer[..length])
        {
            // String ctor stops at the first null terminator.
            result =
                length >= 2 && pBuffer[1] == '\0'
                ? new((char*)pBuffer)
                : new((sbyte*)pBuffer);

            ArrayPool<byte>.Shared.ReturnIfNotNull(rented);

            return true;
        }
    }
}
