using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
{
    public string ReadSizedString(int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadSizedString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, stringType, MainModule, baseOffset, offsets);
        return result;
    }

    public string ReadSizedString(string moduleName, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, ReadStringType.AutoDetect, Modules[moduleName], baseOffset, offsets);
        return result;
    }

    public string ReadSizedString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, stringType, Modules[moduleName], baseOffset, offsets);
        return result;
    }

    public string ReadSizedString(Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, ReadStringType.AutoDetect, module, baseOffset, offsets);
        return result;
    }

    public string ReadSizedString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, stringType, module, baseOffset, offsets);
        return result;
    }

    public string ReadSizedString(nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, ReadStringType.AutoDetect, baseAddress, offsets);
        return result;
    }

    public string ReadSizedString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        _ = TryReadString(out string result, MaxLength, stringType, baseAddress, offsets);
        return result;
    }

    public bool TryReadSizedString(out string result, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
    }

    public bool TryReadSizedString(out string result, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, stringType, MainModule, baseOffset, offsets);
    }

    public bool TryReadSizedString(out string result, string moduleName, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, ReadStringType.AutoDetect, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadSizedString(out string result, ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, stringType, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadSizedString(out string result, Module module, int baseOffset, params int[] offsets)
    {
        return TryReadString(out result, MaxLength, ReadStringType.AutoDetect, module, baseOffset, offsets);
    }

    public bool TryReadSizedString(out string result, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            Debug.Warn($"[ReadString] Module could not be found.");

            result = null;
            return false;
        }

        return TryReadString(out result, MaxLength, stringType, module, baseOffset, offsets);
    }

    public bool TryReadSizedString(out string result, nint baseAddress, params int[] offsets)
    {
        return TryReadString(out result, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public abstract bool TryReadSizedString(out string result, ReadStringType stringType, nint baseAddress, params int[] offsets);
}
