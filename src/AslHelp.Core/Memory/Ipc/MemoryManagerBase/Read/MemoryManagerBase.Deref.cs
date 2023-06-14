namespace AslHelp.Core.Memory.Ipc;

public abstract partial class MemoryManagerBase
{
    public nint Deref(int baseOffset, params int[] offsets)
    {
        return Deref(MainModule, baseOffset, offsets);
    }

    public nint Deref(string moduleName, int baseOffset, params int[] offsets)
    {
        return Deref(Modules[moduleName], baseOffset, offsets);
    }

    public nint Deref(Module? module, int baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            Debug.Warn("[TryDeref] Module could not be found.");

            return default;
        }

        return Deref(module.Base + baseOffset, offsets);
    }

    public abstract nint Deref(nint baseAddress, params int[] offsets);

    public bool TryDeref(out nint result, int baseOffset, params int[] offsets)
    {
        return TryDeref(out result, MainModule, baseOffset, offsets);
    }

    public bool TryDeref(out nint result, string moduleName, int baseOffset, params int[] offsets)
    {
        return TryDeref(out result, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryDeref(out nint result, Module? module, int baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            Debug.Warn("[TryDeref] Module could not be found.");

            result = default;
            return false;
        }

        return TryDeref(out result, module.Base + baseOffset, offsets);
    }

    public abstract bool TryDeref(out nint result, nint baseAddress, params int[] offsets);
}
