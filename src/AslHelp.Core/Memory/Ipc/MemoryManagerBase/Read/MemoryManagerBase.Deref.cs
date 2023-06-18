using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint Deref(uint baseOffset, params int[] offsets)
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = "[Deref] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return Deref(module, baseOffset, offsets);
    }

    public nuint Deref(string moduleName, uint baseOffset, params int[] offsets)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[Deref] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return Deref(module, baseOffset, offsets);
    }

    public nuint Deref(Module module, uint baseOffset, params int[] offsets)
    {
        return Deref(module.Base + baseOffset, offsets);
    }

    public abstract nuint Deref(nuint baseAddress, params int[] offsets);

    public bool TryDeref(out nuint result, uint baseOffset, params int[] offsets)
    {
        return TryDeref(out result, MainModule, baseOffset, offsets);
    }

    public bool TryDeref(out nuint result, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets)
    {
        if (moduleName is null)
        {
            result = default;
            return false;
        }

        return TryDeref(out result, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryDeref(out nuint result, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            result = default;
            return false;
        }

        return TryDeref(out result, module.Base + baseOffset, offsets);
    }

    public abstract bool TryDeref(out nuint result, nuint baseAddress, params int[] offsets);
}