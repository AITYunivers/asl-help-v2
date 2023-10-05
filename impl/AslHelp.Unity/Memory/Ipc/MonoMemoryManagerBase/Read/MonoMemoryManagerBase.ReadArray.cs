using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Memory;

namespace AslHelp.Unity.Memory.Ipc;

public partial class MonoMemoryManagerBase
{
    public T[] ReadArray<T>(uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return ReadArray<T>(MainModule.Base + baseOffset, offsets);
    }

    public T[] ReadArray<T>(string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return ReadArray<T>(Modules[moduleName].Base + baseOffset, offsets);
    }

    public T[] ReadArray<T>(Module module, uint baseOffset, int[] offsets) where T : unmanaged
    {
        return ReadArray<T>(module.Base + baseOffset, offsets);
    }

    public T[] ReadArray<T>(nuint address, params int[] offsets) where T : unmanaged
    {
        nuint deref = Read<nuint>(address, offsets);

        int length = Read<int>(deref + (PointerSize * 3U));
        return ReadSpan<T>(length, deref + (PointerSize * 4U));
    }

    public bool TryReadArray<T>([NotNullWhen(true)] out T[]? results, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryReadArray<T>(out results, MainModule.Base + baseOffset, offsets);
    }

    public bool TryReadArray<T>([NotNullWhen(true)] out T[]? results, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            results = default;
            return false;
        }

        return TryReadArray<T>(out results, Modules[moduleName].Base + baseOffset, offsets);
    }

    public bool TryReadArray<T>([NotNullWhen(true)] out T[]? results, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            results = default;
            return false;
        }

        return TryReadArray<T>(out results, module.Base + baseOffset, offsets);
    }

    public bool TryReadArray<T>([NotNullWhen(true)] out T[]? results, nuint address, params int[] offsets) where T : unmanaged
    {
        if (!TryRead<nuint>(out nuint deref, address, offsets))
        {
            results = default;
            return false;
        }

        if (!TryRead<int>(out int length, deref + (PointerSize * 3U)))
        {
            results = default;
            return false;
        }

        return TryReadSpan<T>(out results, length, deref + (PointerSize * 4U));
    }
}
