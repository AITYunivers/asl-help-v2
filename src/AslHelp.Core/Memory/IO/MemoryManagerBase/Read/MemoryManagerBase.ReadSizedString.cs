using AslHelp.Core.Extensions;
using System.Text;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
{
    public string ReadSizedString(int baseOffset, params int[] offsets)
    {
        _ = TryReadSizedString(out string result, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadSizedString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        _ = TryReadSizedString(out string result, stringType, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadSizedString(string moduleName, int baseOffset, params int[] offsets)
    {
        _ = TryReadSizedString(out string result, ReadStringType.AutoDetect, Modules[moduleName], baseOffset, offsets);
        return result;
    }

    public string ReadSizedString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        _ = TryReadSizedString(out string result, stringType, Modules[moduleName], baseOffset, offsets);
        return result;
    }

    public string ReadSizedString(Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadSizedString(out string result, ReadStringType.AutoDetect, module, baseOffset, offsets);
        return result;
    }

    public string ReadSizedString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadSizedString(out string result, stringType, module, baseOffset, offsets);
        return result;
    }

    public string ReadSizedString(nint baseAddress, params int[] offsets)
    {
        _ = TryReadSizedString(out string result, ReadStringType.AutoDetect, baseAddress, offsets);
        return result;
    }

    public string ReadSizedString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        _ = TryReadSizedString(out string result, stringType, baseAddress, offsets);
        return result;
    }

    public bool TryReadSizedString(out string result, int baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
    }

    public bool TryReadSizedString(out string result, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, stringType, MainModule, baseOffset, offsets);
    }

    public bool TryReadSizedString(out string result, string moduleName, int baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, ReadStringType.AutoDetect, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadSizedString(out string result, ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, stringType, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadSizedString(out string result, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadSizedString(out result, ReadStringType.AutoDetect, module, baseOffset, offsets);
    }

    public bool TryReadSizedString(out string result, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            Debug.Warn($"[ReadString] Module could not be found.");

            result = null;
            return false;
        }

        return TryReadSizedString(out result, stringType, module, baseOffset, offsets);
    }

    public bool TryReadSizedString(out string result, nint baseAddress, params int[] offsets)
    {
        return TryReadSizedString(out result, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public bool TryReadSizedString(out string result, ReadStringType stringType, nint baseAddress, params int[] offsets)
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

    private unsafe bool InternalTryReadSizedString(out string result, nint baseAddress, int[] offsets)
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

        sbyte[] rented = null;
        Span<sbyte> buffer =
            length <= 1024
            ? stackalloc sbyte[1024]
            : (rented = ArrayPoolExtensions.Rent<sbyte>(length));

        if (!TryReadSpan(buffer, deref))
        {
            result = default;
            return false;
        }

        fixed (sbyte* pBuffer = buffer)
        {
            result = new string(pBuffer);
            ArrayPoolExtensions.Return(rented);

            return true;
        }
    }

    private unsafe bool InternalTryReadSizedWideString(out string result, nint baseAddress, int[] offsets)
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

        char[] rented = null;
        Span<char> buffer =
            length <= 512
            ? stackalloc char[512]
            : (rented = ArrayPoolExtensions.Rent<char>(length));

        if (!TryReadSpan(buffer, deref))
        {
            result = default;
            return false;
        }

        fixed (char* pBuffer = buffer)
        {
            result = new string(pBuffer);
            ArrayPoolExtensions.Return(rented);

            return true;
        }
    }

    private unsafe bool InternalTryReadSizedAutoString(out string result, nint baseAddress, int[] offsets)
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
        byte[] rented = null;
        Span<byte> buffer =
            unicodeLength <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPoolExtensions.Rent<byte>(unicodeLength));

        if (!TryReadSpan(buffer, baseAddress, offsets))
        {
            ArrayPoolExtensions.Return(rented);

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

            ArrayPoolExtensions.Return(rented);

            return true;
        }
    }
}
