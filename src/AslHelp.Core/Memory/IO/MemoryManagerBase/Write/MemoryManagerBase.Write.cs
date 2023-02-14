namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
{
    public bool Write<T>(T value, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return Write<T>(value, MainModule, baseOffset, offsets);
    }

    public bool Write<T>(T value, string module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return Write<T>(value, Modules[module], baseOffset, offsets);
    }

    public bool Write<T>(T value, Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            Debug.Warn($"[Write<{typeof(T).Name}>] Module could not be found.");

            return false;
        }

        return Write<T>(value, module.Base + baseOffset, offsets);
    }

    public abstract bool Write<T>(T value, nint baseAddress, params int[] offsets) where T : unmanaged;
}
