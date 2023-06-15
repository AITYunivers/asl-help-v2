using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Ipc;

public abstract partial class MemoryManagerBase
{
    public string? ReadSizedString(int baseOffset, params int[] offsets)
    {
        return ReadSizedString(ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
    }

    public string? ReadSizedString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return ReadSizedString(stringType, MainModule, baseOffset, offsets);
    }

    public string? ReadSizedString(string moduleName, int baseOffset, params int[] offsets)
    {
        return ReadSizedString(ReadStringType.AutoDetect, Modules[moduleName], baseOffset, offsets);
    }

    public string? ReadSizedString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        return ReadSizedString(stringType, Modules[moduleName], baseOffset, offsets);
    }

    public string? ReadSizedString(Module? module, int baseOffset, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(module, nameof(module));

        return ReadSizedString(ReadStringType.AutoDetect, module, baseOffset, offsets);
    }

    public string? ReadSizedString(ReadStringType stringType, Module? module, int baseOffset, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(module, nameof(module));


    }

    public string? ReadSizedString(nint baseAddress, params int[] offsets)
    {
        return ReadSizedString(ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public string? ReadSizedString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {

    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, int baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, stringType, MainModule, baseOffset, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, string moduleName, int baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, ReadStringType.AutoDetect, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, stringType, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, Module? module, int baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, ReadStringType.AutoDetect, module, baseOffset, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, Module? module, int baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            Debug.Warn("[TryReadString] Module could not be found.");

            result = null;
            return false;
        }

        return TryReadSizedString(out result, stringType, module, baseOffset, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, nint baseAddress, params int[] offsets)
    {
        return TryReadSizedString(out result, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        if (stringType == ReadStringType.AutoDetect)
        {
            return InternalTryReadSizedAutoString(out result, baseAddress, offsets);
        }
        else if (stringType == ReadStringType.UTF16)
        {
            return InternalTryReadSizedWideString(out result, baseAddress, offsets);
        }
        else
        {
            return InternalTryReadSizedString(out result, baseAddress, offsets);
        }
    }

    private unsafe bool InternalTryReadSizedString([NotNullWhen(true)] out string? result, nint baseAddress, int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            result = default;
            return false;
        }

        if (!TryRead(out int size, deref - 0x4))
        {
            result = default;
            return false;
        }

        int length = size;

        sbyte[]? rented = null;
        Span<sbyte> buffer =
            length <= 1024
            ? stackalloc sbyte[1024]
            : (rented = ArrayPool<sbyte>.Shared.Rent(length));

        if (!TryReadSpan(buffer, deref))
        {
            result = default;
            return false;
        }

        fixed (sbyte* pBuffer = buffer)
        {
            result = new string(pBuffer);
            ArrayPool<sbyte>.Shared.ReturnIfNotNull(rented);

            return true;
        }
    }

    private unsafe bool InternalTryReadSizedWideString([NotNullWhen(true)] out string? result, nint baseAddress, int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            result = default;
            return false;
        }

        if (!TryRead(out int length, deref - 0x4))
        {
            result = default;
            return false;
        }

        char[]? rented = null;
        Span<char> buffer =
            length <= 512
            ? stackalloc char[512]
            : (rented = ArrayPool<char>.Shared.Rent(length));

        if (!TryReadSpan(buffer, deref))
        {
            result = default;
            return false;
        }

        fixed (char* pBuffer = buffer)
        {
            result = new string(pBuffer);
            ArrayPool<char>.Shared.ReturnIfNotNull(rented);

            return true;
        }
    }

    private unsafe bool InternalTryReadSizedAutoString([NotNullWhen(true)] out string? result, nint baseAddress, int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            result = default;
            return false;
        }

        if (!TryRead(out int size, deref - 0x4))
        {
            result = default;
            return false;
        }

        int utf8Length = size, unicodeLength = size * 2;

        // Assume unicode for the worst-case scenario and just allocate length * 2.
        byte[]? rented = null;
        Span<byte> buffer =
            unicodeLength <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPool<byte>.Shared.Rent(unicodeLength));

        if (!TryReadSpan(buffer, baseAddress, offsets))
        {
            ArrayPool<byte>.Shared.ReturnIfNotNull(rented);

            result = default;
            return false;
        }

        fixed (byte* pBuffer = buffer)
        {
            // String ctor stops at the first null terminator.
            result =
                utf8Length >= 2 && pBuffer[1] == '\0'
                ? new string((char*)pBuffer)
                : new string((sbyte*)pBuffer);

            ArrayPool<byte>.Shared.ReturnIfNotNull(rented);

            return true;
        }
    }
}
