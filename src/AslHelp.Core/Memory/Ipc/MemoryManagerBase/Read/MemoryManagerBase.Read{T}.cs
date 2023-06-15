using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public T Read<T>(int baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = $"[Read<{typeof(T).Name}>] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return Read<T>(module, baseOffset, offsets);
    }

    public T Read<T>(string moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[Read<{typeof(T).Name}>] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return Read<T>(module, baseOffset, offsets);
    }

    public T Read<T>(Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return Read<T>(module.Base + baseOffset, offsets);
    }

    public abstract T Read<T>(nint baseAddress, params int[] offsets) where T : unmanaged;

    public bool TryRead<T>(out T result, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryRead(out result, MainModule, baseOffset, offsets);
    }

    public bool TryRead<T>(out T result, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            result = default;
            return false;
        }

        return TryRead(out result, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryRead<T>(out T result, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            result = default;
            return false;
        }

        return TryRead(out result, module.Base + baseOffset, offsets);
    }

    public abstract bool TryRead<T>(out T result, nint baseAddress, params int[] offsets) where T : unmanaged;
}
