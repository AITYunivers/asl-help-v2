using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
{
    private const int MaxLength = 256;

    public string ReadString(int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, ReadStringType.AutoDetect, false, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(int baseOffset, bool sized, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, ReadStringType.AutoDetect, sized, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, ReadStringType.AutoDetect, false, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, bool sized, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, ReadStringType.AutoDetect, sized, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, stringType, false, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, bool sized, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, stringType, sized, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, stringType, false, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, bool sized, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, stringType, sized, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(string module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, ReadStringType.AutoDetect, false, Modules[module], baseOffset, offsets);
        return result;
    }

    public string ReadString(bool sized, string module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, ReadStringType.AutoDetect, sized, Modules[module], baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, string module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, ReadStringType.AutoDetect, false, Modules[module], baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, bool sized, string module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, ReadStringType.AutoDetect, sized, Modules[module], baseOffset, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, stringType, false, Modules[module], baseOffset, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, bool sized, string module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, stringType, sized, Modules[module], baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {

        _ = TryReadString(out string result, length, stringType, false, Modules[module], baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, bool sized, string module, int baseOffset, params int[] offsets)
    {

        _ = TryReadString(out string result, length, stringType, sized, Modules[module], baseOffset, offsets);
        return result;
    }

    public string ReadString(Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, ReadStringType.AutoDetect, false, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(bool sized, Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, ReadStringType.AutoDetect, sized, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, ReadStringType.AutoDetect, false, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, bool sized, Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, ReadStringType.AutoDetect, sized, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, stringType, false, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, bool sized, Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, stringType, sized, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, stringType, false, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, bool sized, Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, length, stringType, sized, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, ReadStringType.AutoDetect, false, baseAddress, offsets);
        return result;
    }

    public string ReadString(bool sized, nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, ReadStringType.AutoDetect, sized, baseAddress, offsets);
        return result;
    }

    public string ReadString(int length, nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, length, ReadStringType.AutoDetect, false, baseAddress, offsets);
        return result;
    }

    public string ReadString(int length, bool sized, nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, length, ReadStringType.AutoDetect, sized, baseAddress, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, stringType, false, baseAddress, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, bool sized, nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, stringType, sized, baseAddress, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, length, stringType, false, baseAddress, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, bool sized, nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, length, stringType, sized, baseAddress, offsets);
        return result;
    }

    public bool TryReadString(out string result, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, ReadStringType.AutoDetect, false, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, bool sized, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, ReadStringType.AutoDetect, sized, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, false, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, bool sized, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, sized, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, stringType, false, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, bool sized, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, stringType, sized, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, stringType, false, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, bool sized, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, stringType, sized, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, string module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, ReadStringType.AutoDetect, false, Modules[module], baseOffset, offsets);
    }

    public bool TryReadString(out string result, bool sized, string module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, ReadStringType.AutoDetect, sized, Modules[module], baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, string module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, false, Modules[module], baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, bool sized, string module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, sized, Modules[module], baseOffset, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, stringType, false, Modules[module], baseOffset, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, bool sized, string module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, stringType, sized, Modules[module], baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, stringType, false, Modules[module], baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, bool sized, string module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, stringType, sized, Modules[module], baseOffset, offsets);
    }

    public bool TryReadString(out string result, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, ReadStringType.AutoDetect, false, module, baseOffset, offsets);
    }

    public bool TryReadString(out string result, bool sized, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, ReadStringType.AutoDetect, sized, module, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, false, module, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, bool sized, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, sized, module, baseOffset, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, stringType, false, module, baseOffset, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, bool sized, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, stringType, sized, module, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, stringType, false, module, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, bool sized, Module module, int baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            Debug.Warn($"[ReadString] Module could not be found.");

            result = null;
            return false;
        }

        return TryReadString(out result, length, stringType, sized, module.Base + baseOffset, offsets);
    }

    public bool TryReadString(out string result, nint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, ReadStringType.AutoDetect, false, baseAddress, offsets);
    }

    public bool TryReadString(out string result, bool sized, nint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, ReadStringType.AutoDetect, sized, baseAddress, offsets);
    }

    public bool TryReadString(out string result, int length, nint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, false, baseAddress, offsets);
    }

    public bool TryReadString(out string result, int length, bool sized, nint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, sized, baseAddress, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, stringType, false, baseAddress, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, bool sized, nint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, stringType, sized, baseAddress, offsets);
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, length, stringType, false, baseAddress, offsets);
    }

    public abstract bool TryReadString(out string result, int length, ReadStringType stringType, bool sized, nint baseAddress, params int[] offsets);
}
