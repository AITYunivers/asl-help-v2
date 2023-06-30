using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    // ReadSizedString

    public string ReadSizedString(uint baseOffset, params int[] offsets)
    {
        return ReadSizedString(ReadStringType.AutoDetect, baseOffset, offsets);
    }

    public string ReadSizedString(ReadStringType stringType, uint baseOffset, params int[] offsets)
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = "[ReadSizedString] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ReadSizedString(stringType, module, baseOffset, offsets);
    }

    public string ReadSizedString(string moduleName, uint baseOffset, params int[] offsets)
    {
        return ReadSizedString(ReadStringType.AutoDetect, moduleName, baseOffset, offsets);
    }

    public string ReadSizedString(ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ReadSizedString] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ReadSizedString(stringType, module, baseOffset, offsets);
    }

    public string ReadSizedString(Module module, uint baseOffset, params int[] offsets)
    {
        return ReadSizedString(ReadStringType.AutoDetect, module, baseOffset, offsets);
    }

    public string ReadSizedString(ReadStringType stringType, Module module, uint baseOffset, params int[] offsets)
    {
        return ReadSizedString(stringType, module.Base + baseOffset, offsets);
    }

    public string ReadSizedString(nuint baseAddress, params int[] offsets)
    {
        return ReadSizedString(ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public string ReadSizedString(ReadStringType stringType, nuint baseAddress, params int[] offsets)
    {
        if (stringType == ReadStringType.AutoDetect)
        {
            return InternalReadSizedAutoString(baseAddress, offsets);
        }
        else if (stringType == ReadStringType.UTF16)
        {
            return InternalReadSizedWideString(baseAddress, offsets);
        }
        else
        {
            return InternalReadSizedString(baseAddress, offsets);
        }
    }

    private unsafe string InternalReadSizedString(nuint baseAddress, int[] offsets)
    {
        nuint deref = Deref(baseAddress, offsets);
        int length = Read<int>(deref - 0x4);

        sbyte[]? rented = null;
        Span<sbyte> buffer =
            length <= 1024
            ? stackalloc sbyte[1024]
            : (rented = ArrayPool<sbyte>.Shared.Rent(length));

        ReadSpan(buffer, deref);

        fixed (sbyte* pBuffer = buffer)
        {
            // String ctor stops at the first null terminator.
            string result = new(pBuffer);
            ArrayPool<sbyte>.Shared.ReturnIfNotNull(rented);

            return result;
        }
    }

    private unsafe string InternalReadSizedWideString(nuint baseAddress, int[] offsets)
    {
        nuint deref = Deref(baseAddress, offsets);
        int length = Read<int>(deref - 0x4);

        char[]? rented = null;
        Span<char> buffer =
            length <= 1024
            ? stackalloc char[1024]
            : (rented = ArrayPool<char>.Shared.Rent(length));

        ReadSpan(buffer, deref);

        string result = buffer[..length].ToString();
        ArrayPool<char>.Shared.ReturnIfNotNull(rented);

        return result;
    }

    private unsafe string InternalReadSizedAutoString(nuint baseAddress, int[] offsets)
    {
        nuint deref = Deref(baseAddress, offsets);
        int size = Read<int>(deref - 0x4);

        int utf8Length = size, unicodeLength = size * 2;

        // Assume unicode for the worst-case scenario and just allocate length * 2.
        byte[]? rented = null;
        Span<byte> buffer =
            unicodeLength <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPool<byte>.Shared.Rent(unicodeLength));

        ReadSpan(buffer, deref);

        fixed (byte* pBuffer = buffer)
        {
            if (utf8Length >= 2 && pBuffer[1] == '\0')
            {
                string result = new ReadOnlySpan<char>(pBuffer, size).ToString();
                ArrayPool<byte>.Shared.ReturnIfNotNull(rented);

                return result;
            }
            else
            {
                // String ctor stops at the first null terminator.
                string result = new((sbyte*)pBuffer);
                ArrayPool<byte>.Shared.ReturnIfNotNull(rented);

                return result;
            }
        }
    }

    // TryReadSizedString

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, uint baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, ReadStringType.AutoDetect, baseOffset, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, uint baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, stringType, MainModule, baseOffset, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, ReadStringType.AutoDetect, moduleName, baseOffset, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets)
    {
        if (moduleName is null)
        {
            result = default;
            return false;
        }

        return TryReadSizedString(out result, stringType, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, ReadStringType.AutoDetect, module, baseOffset, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            result = default;
            return false;
        }

        return TryReadSizedString(out result, stringType, module.Base + baseOffset, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, nuint baseAddress, params int[] offsets)
    {
        return TryReadSizedString(out result, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, nuint baseAddress, params int[] offsets)
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

    private unsafe bool InternalTryReadSizedString(out string? result, nuint baseAddress, int[] offsets)
    {
        if (!TryDeref(out nuint deref, baseAddress, offsets))
        {
            result = default;
            return false;
        }

        if (!TryRead(out int length, deref - 0x4))
        {
            result = default;
            return false;
        }

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
            // String ctor stops at the first null terminator.
            result = new(pBuffer);
            ArrayPool<sbyte>.Shared.ReturnIfNotNull(rented);

            return true;
        }
    }

    private unsafe bool InternalTryReadSizedWideString(out string? result, nuint baseAddress, int[] offsets)
    {
        if (!TryDeref(out nuint deref, baseAddress, offsets))
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
            // String ctor stops at the first null terminator.
            result = new(pBuffer);
            ArrayPool<char>.Shared.ReturnIfNotNull(rented);

            return true;
        }
    }

    private unsafe bool InternalTryReadSizedAutoString(out string? result, nuint baseAddress, int[] offsets)
    {
        if (!TryDeref(out nuint deref, baseAddress, offsets))
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

        if (!TryReadSpan(buffer, deref))
        {
            ArrayPool<byte>.Shared.ReturnIfNotNull(rented);

            result = default;
            return false;
        }

        fixed (byte* pBuffer = buffer)
        {
            if (utf8Length >= 2 && pBuffer[1] == '\0')
            {
                result = new ReadOnlySpan<char>(pBuffer, unicodeLength).ToString();
                ArrayPool<byte>.Shared.ReturnIfNotNull(rented);

                return true;
            }
            else
            {
                // String ctor stops at the first null terminator.
                result = new((sbyte*)pBuffer);
                ArrayPool<byte>.Shared.ReturnIfNotNull(rented);

                return true;
            }
        }
    }
}
