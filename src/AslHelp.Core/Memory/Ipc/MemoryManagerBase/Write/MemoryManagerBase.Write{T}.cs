using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public void Write<T>(T value, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = $"[Write<{typeof(T).Name}>] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Write<T>(value, module, baseOffset, offsets);
    }

    public void Write<T>(T value, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[Write<{typeof(T).Name}>] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Write<T>(value, module, baseOffset, offsets);
    }

    public void Write<T>(T value, Module module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Write<T>(value, module.Base + baseOffset, offsets);
    }

    public abstract void Write<T>(T value, nuint baseAddress, params int[] offsets) where T : unmanaged;

    public bool TryWrite<T>(T value, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryWrite<T>(value, MainModule, baseOffset, offsets);
    }

    public bool TryWrite<T>(T value, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return TryWrite<T>(value, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryWrite<T>(T value, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return TryWrite<T>(value, module.Base + baseOffset, offsets);
    }

    public abstract bool TryWrite<T>(T value, nuint baseAddress, params int[] offsets) where T : unmanaged;
}
