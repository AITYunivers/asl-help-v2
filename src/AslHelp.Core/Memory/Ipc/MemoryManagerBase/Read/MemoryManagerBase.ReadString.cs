using System;
using System.Buffers;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Ipc;

public abstract partial class MemoryManagerBase
{
    private const byte AsciiNullChar = 0;
    private static ReadOnlySpan<byte> UnicodeNullChar => new byte[] { 0, 0 };

    public string ReadString(int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, AHR.MaxStringReadLength, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, AHR.MaxStringReadLength, stringType, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, stringType, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(string moduleName, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, AHR.MaxStringReadLength, ReadStringType.AutoDetect, Modules[moduleName], baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, string moduleName, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, ReadStringType.AutoDetect, Modules[moduleName], baseOffset, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, AHR.MaxStringReadLength, stringType, Modules[moduleName], baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, stringType, Modules[moduleName], baseOffset, offsets);
        return result;
    }

    public string ReadString(Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, AHR.MaxStringReadLength, ReadStringType.AutoDetect, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, ReadStringType.AutoDetect, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, AHR.MaxStringReadLength, stringType, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, stringType, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, AHR.MaxStringReadLength, ReadStringType.AutoDetect, baseAddress, offsets);
        return result;
    }

    public string ReadString(int length, nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, length, ReadStringType.AutoDetect, baseAddress, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, AHR.MaxStringReadLength, stringType, baseAddress, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, length, stringType, baseAddress, offsets);
        return result;
    }

    public bool TryReadString(out string result, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, AHR.MaxStringReadLength, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, AHR.MaxStringReadLength, stringType, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, stringType, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, string moduleName, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, AHR.MaxStringReadLength, ReadStringType.AutoDetect, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, string moduleName, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, AHR.MaxStringReadLength, stringType, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, stringType, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadString(out string result, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, AHR.MaxStringReadLength, ReadStringType.AutoDetect, module, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, module, baseOffset, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, AHR.MaxStringReadLength, stringType, module, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            Debug.Warn($"[ReadString] Module could not be found.");

            result = null;
            return false;
        }

        return TryReadString(out result, length, stringType, module.Base + baseOffset, offsets);
    }

    public bool TryReadString(out string result, nint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, AHR.MaxStringReadLength, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public bool TryReadString(out string result, int length, nint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, AHR.MaxStringReadLength, stringType, baseAddress, offsets);
    }

    public unsafe bool TryReadString(out string result, int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
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

    private unsafe bool InternalTryReadString(out string result, int length, nint baseAddress, int[] offsets)
    {
        sbyte[] rented = null;
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

        fixed (sbyte* pBuffer = buffer)
        {
            result = new string(pBuffer);
            ArrayPool<sbyte>.Shared.ReturnIfNotNull(rented);

            return true;
        }
    }

    private unsafe bool InternalTryReadWideString(out string result, int length, nint baseAddress, int[] offsets)
    {
        length *= 2;

        char[] rented = null;
        Span<char> buffer =
            length <= 512
            ? stackalloc char[512]
            : (rented = ArrayPoolExtensions.Rent<char>(length));

        if (!TryReadSpan(buffer, baseAddress, offsets))
        {
            ArrayPoolExtensions.ReturnIfNotNull(rented);

            result = default;
            return false;
        }

        fixed (char* pBuffer = buffer)
        {
            result = new string(pBuffer);
            ArrayPoolExtensions.ReturnIfNotNull(rented);

            return true;
        }
    }

    private unsafe bool InternalTryReadAutoString(out string result, int length, nint baseAddress, int[] offsets)
    {
        // Assume unicode for the worst-case scenario and just allocate length * 2.
        byte[] rented = null;
        Span<byte> buffer =
            length * 2 <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPoolExtensions.Rent<byte>(length * 2));

        if (!TryReadSpan(buffer, baseAddress, offsets))
        {
            ArrayPoolExtensions.ReturnIfNotNull(rented);

            result = default;
            return false;
        }

        fixed (byte* pBuffer = buffer)
        {
            // String ctor stops at the first null terminator.
            result =
                length >= 2 && pBuffer[1] == '\0'
                ? new string((char*)pBuffer)
                : new string((sbyte*)pBuffer);

            ArrayPoolExtensions.ReturnIfNotNull(rented);

            return true;
        }
    }
}
