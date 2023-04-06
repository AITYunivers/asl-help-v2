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

    public abstract bool TryReadSizedString(out string result, ReadStringType stringType, nint baseAddress, params int[] offsets);
}
