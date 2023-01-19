namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryIOBase
{
    public nint Deref(int baseOffset, params int[] offsets)
    {
        TryDeref(out nint result, _manager.MainModule, baseOffset, offsets);
        return result;
    }

    public nint Deref(string module, int baseOffset, params int[] offsets)
    {
        TryDeref(out nint result, _manager.Modules[module], baseOffset, offsets);
        return result;
    }

    public nint Deref(Module module, int baseOffset, params int[] offsets)
    {
        TryDeref(out nint result, module, baseOffset, offsets);
        return result;
    }

    public nint Deref(nint baseAddress, params int[] offsets)
    {
        TryDeref(out nint result, baseAddress, offsets);
        return result;
    }

    public bool TryDeref(out nint result, int baseOffset, params int[] offsets)
    {
        return TryDeref(out result, _manager.MainModule, baseOffset, offsets);
    }

    public bool TryDeref(out nint result, string module, int baseOffset, params int[] offsets)
    {
        return TryDeref(out result, _manager.Modules[module], baseOffset, offsets);
    }

    public bool TryDeref(out nint result, Module module, int baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            Debug.Warn("[Deref] Module could not be found.");

            result = default;
            return false;
        }

        return TryDeref(out result, module.Base + baseOffset, offsets);
    }

    public abstract bool TryDeref(out nint result, nint baseAddress, params int[] offsets);
}
