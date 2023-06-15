using System.Diagnostics.CodeAnalysis;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public bool Write<T>(T value, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return Write<T>(value, MainModule, baseOffset, offsets);
    }

    public bool Write<T>(T value, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return Write<T>(value, Modules[moduleName], baseOffset, offsets);
    }

    public bool Write<T>(T value, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return Write<T>(value, module.Base + baseOffset, offsets);
    }

    public abstract bool Write<T>(T value, nint baseAddress, params int[] offsets) where T : unmanaged;
}
