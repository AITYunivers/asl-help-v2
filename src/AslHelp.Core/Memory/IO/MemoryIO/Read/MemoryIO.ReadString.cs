using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryIO
{
    public string ReadString(int baseOffset, params int[] offsets)
    {
        TryReadString(out string result, 128, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, int baseOffset, params int[] offsets)
    {
        TryReadString(out string result, length, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        TryReadString(out string result, 128, stringType, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        TryReadString(out string result, length, stringType, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadString(string module, int baseOffset, params int[] offsets)
    {
        TryReadString(out string result, 128, ReadStringType.AutoDetect, Modules[module], baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, string module, int baseOffset, params int[] offsets)
    {
        TryReadString(out string result, length, ReadStringType.AutoDetect, Modules[module], baseOffset, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        TryReadString(out string result, 128, stringType, Modules[module], baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {

        TryReadString(out string result, length, stringType, Modules[module], baseOffset, offsets);
        return result;
    }

    public string ReadString(Module module, int baseOffset, params int[] offsets)
    {
        TryReadString(out string result, 128, ReadStringType.AutoDetect, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, Module module, int baseOffset, params int[] offsets)
    {
        TryReadString(out string result, length, ReadStringType.AutoDetect, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        TryReadString(out string result, 128, stringType, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        TryReadString(out string result, length, stringType, module, baseOffset, offsets);
        return result;
    }

    public string ReadString(nint baseAddress, params int[] offsets)
    {
        TryReadString(out string result, 128, ReadStringType.AutoDetect, baseAddress, offsets);
        return result;
    }

    public string ReadString(int length, nint baseAddress, params int[] offsets)
    {
        TryReadString(out string result, length, ReadStringType.AutoDetect, baseAddress, offsets);
        return result;
    }

    public string ReadString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        TryReadString(out string result, 128, stringType, baseAddress, offsets);
        return result;
    }

    public string ReadString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        TryReadString(out string result, length, stringType, baseAddress, offsets);
        return result;
    }

    public bool TryReadString(out string result, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, 128, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, 128, stringType, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, stringType, MainModule, baseOffset, offsets);
    }

    public bool TryReadString(out string result, string module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, 128, ReadStringType.AutoDetect, Modules[module], baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, string module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, Modules[module], baseOffset, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, 128, stringType, Modules[module], baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, stringType, Modules[module], baseOffset, offsets);
    }

    public bool TryReadString(out string result, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, 128, ReadStringType.AutoDetect, module, baseOffset, offsets);
    }

    public bool TryReadString(out string result, int length, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, module, baseOffset, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, 128, stringType, module, baseOffset, offsets);
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
        return TryReadString(out result, 128, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public bool TryReadString(out string result, int length, nint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, length, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public bool TryReadString(out string result, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, 128, stringType, baseAddress, offsets);
    }

    public abstract bool TryReadString(out string result, int length, ReadStringType stringType, nint baseAddress, params int[] offsets);
}
