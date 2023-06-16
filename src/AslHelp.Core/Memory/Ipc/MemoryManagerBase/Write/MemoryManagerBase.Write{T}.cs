using System.Diagnostics.CodeAnalysis;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public bool Write<T>(T value, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return Write<T>(value, MainModule, baseOffset, offsets);
    }

    public bool Write<T>(T value, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return Write<T>(value, Modules[moduleName], baseOffset, offsets);
    }

    public bool Write<T>(T value, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return Write<T>(value, module.Base + baseOffset, offsets);
    }

    public abstract bool Write<T>(T value, nuint baseAddress, params int[] offsets) where T : unmanaged;
}
