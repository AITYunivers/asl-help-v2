using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Memory;

using LiveSplit.ComponentUtil;

namespace AslHelp.Mono.Memory.Ipc;

public partial class MonoMemoryManagerBase
{
    public string ReadString(uint baseOffset, params int[] offsets)
    {
        Module module = MainModule;
        return ReadString(module.Base + baseOffset, offsets);
    }

    public string ReadString(string moduleName, uint baseOffset, params int[] offsets)
    {
        Module module = Modules[moduleName];
        return ReadString(module.Base + baseOffset, offsets);
    }

    public string ReadString(Module module, uint baseOffset, int[] offsets)
    {
        return ReadString(module.Base + baseOffset, offsets);
    }

    public string ReadString(nuint address, params int[] offsets)
    {
        nuint deref = Read<nuint>(address, offsets);
        return ReadSizedString(ReadStringType.UTF16, deref + (PtrSize * 2U) + 4U);
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, uint baseOffset, params int[] offsets)
    {
        Module module = MainModule;
        return TryReadString(out result, module.Base + baseOffset, offsets);
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets)
    {
        if (moduleName is null)
        {
            result = default;
            return false;
        }

        Module module = Modules[moduleName];
        return TryReadString(out result, module.Base + baseOffset, offsets);
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets)
    {
        if (module is null)
        {
            result = default;
            return false;
        }

        return TryReadString(out result, module.Base + baseOffset, offsets);
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, nuint address, params int[] offsets)
    {
        if (!TryRead<nuint>(out nuint deref, address, offsets))
        {
            result = default;
            return false;
        }

        return TryReadSizedString(out result, ReadStringType.UTF16, deref + (PtrSize * 2U) + 4U);
    }
}
