using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Results;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint Deref(uint baseOffset, params int[] offsets)
    {
        return Deref(MainModule.Base + baseOffset, offsets);
    }

    public nuint Deref(string moduleName, uint baseOffset, params int[] offsets)
    {
        return Deref(Modules[moduleName].Base + baseOffset, offsets);
    }

    public nuint Deref(Module module, uint baseOffset, params int[] offsets)
    {
        return Deref(module.Base + baseOffset, offsets);
    }

    public nuint Deref(nuint baseAddress, params int[] offsets)
    {
        Result<nuint> derefResult = TryDeref(baseAddress, offsets);

        if (!derefResult.IsSuccess)
        {
            derefResult.Throw();
        }

        return derefResult.Value;
    }

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

        return TryDeref(out result, Modules[moduleName].Base + baseOffset, offsets);
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

    public bool TryDeref(out nuint result, nuint baseAddress, params int[] offsets)
    {
        Result<nuint> derefResult = TryDeref(baseAddress, offsets);

        result = derefResult.Value;
        return derefResult.IsSuccess;
    }

    protected abstract Result<nuint> TryDeref(nuint baseAddress, ReadOnlySpan<int> offsets);
}
