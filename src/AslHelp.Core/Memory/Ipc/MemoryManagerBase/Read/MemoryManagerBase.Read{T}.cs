using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public T Read<T>(uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = $"[Read<{typeof(T).Name}>] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return Read<T>(module, baseOffset, offsets);
    }

    public T Read<T>(string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[Read<{typeof(T).Name}>] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return Read<T>(module, baseOffset, offsets);
    }

    public T Read<T>(Module module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return Read<T>(module.Base + baseOffset, offsets);
    }

    public abstract T Read<T>(nuint baseAddress, params int[] offsets) where T : unmanaged;

    public bool TryRead<T>(out T result, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryRead(out result, MainModule, baseOffset, offsets);
    }

    public bool TryRead<T>(out T result, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            result = default;
            return false;
        }

        return TryRead(out result, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryRead<T>(out T result, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            result = default;
            return false;
        }

        return TryRead(out result, module.Base + baseOffset, offsets);
    }

    public abstract bool TryRead<T>(out T result, nuint baseAddress, params int[] offsets) where T : unmanaged;
}
