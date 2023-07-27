using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Memory;

namespace AslHelp.Mono.Memory.Ipc;

public partial class MonoMemoryManagerBase
{
    public void WriteArray<T>(T[] values, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Module module = MainModule;
        WriteArray(values, module.Base + baseOffset, offsets);
    }

    public void WriteArray<T>(T[] values, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Module module = Modules[moduleName];
        WriteArray(values, module.Base + baseOffset, offsets);
    }

    public void WriteArray<T>(T[] values, Module module, uint baseOffset, int[] offsets) where T : unmanaged
    {
        WriteArray(values, module.Base + baseOffset, offsets);
    }

    public void WriteArray<T>(T[] values, nuint address, params int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryWriteArray<T>(T[] values, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Module module = MainModule;
        return TryWriteArray(values, module.Base + baseOffset, offsets);
    }

    public bool TryWriteArray<T>(T[] values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        Module module = Modules[moduleName];
        return TryWriteArray(values, module.Base + baseOffset, offsets);
    }

    public bool TryWriteArray<T>(T[] values, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return TryWriteArray(values, module.Base + baseOffset, offsets);
    }

    public bool TryWriteArray<T>(T[] values, nuint address, params int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }
}
