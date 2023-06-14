namespace AslHelp.Core.Memory.Ipc;

public abstract partial class MemoryManagerBase
{
    public T Read<T>(int baseOffset, params int[] offsets) where T : unmanaged
    {
        _ = TryRead<T>(out T result, MainModule, baseOffset, offsets);
        return result;
    }

    public T Read<T>(string moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        _ = TryRead<T>(out T result, Modules[moduleName], baseOffset, offsets);
        return result;
    }

    public T Read<T>(Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        _ = TryRead<T>(out T result, module, baseOffset, offsets);
        return result;
    }

    public T Read<T>(nint baseAddress, params int[] offsets) where T : unmanaged
    {
        _ = TryRead<T>(out T result, baseAddress, offsets);
        return result;
    }

    public bool TryRead<T>(out T result, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryRead<T>(out result, MainModule, baseOffset, offsets);
    }

    public bool TryRead<T>(out T result, string moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryRead<T>(out result, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryRead<T>(out T result, Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            Debug.Warn($"[Read<{typeof(T).Name}>] Module could not be found.");

            result = default;
            return false;
        }

        return TryRead<T>(out result, module.Base + baseOffset, offsets);
    }

    public abstract bool TryRead<T>(out T result, nint baseAddress, params int[] offsets) where T : unmanaged;
}
