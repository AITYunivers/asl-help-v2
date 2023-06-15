using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public T[] ReadSpan<T>(int length, int baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = $"[ReadSpan<{typeof(T).Name}>] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ReadSpan<T>(length, module, baseOffset, offsets);
    }

    public T[] ReadSpan<T>(int length, string moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ReadSpan<{typeof(T).Name}>] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ReadSpan<T>(length, module, baseOffset, offsets);
    }

    public T[] ReadSpan<T>(int length, Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return ReadSpan<T>(length, module.Base + baseOffset, offsets);
    }

    public T[] ReadSpan<T>(int length, nint baseAddress, params int[] offsets) where T : unmanaged
    {
        ThrowHelper.ThrowIfLessThan(length, 0);

        T[] results = new T[length];
        ReadSpan<T>(results, baseAddress, offsets);

        return results;
    }

    public void ReadSpan<T>(Span<T> buffer, int baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = $"[ReadSpan<{typeof(T).Name}>] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        ReadSpan<T>(buffer, module, baseOffset, offsets);
    }

    public void ReadSpan<T>(Span<T> buffer, string moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ReadSpan<{typeof(T).Name}>] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        ReadSpan<T>(buffer, module, baseOffset, offsets);
    }

    public void ReadSpan<T>(Span<T> buffer, Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        ReadSpan<T>(buffer, module.Base + baseOffset, offsets);
    }

    public abstract void ReadSpan<T>(Span<T> buffer, nint baseAddress, params int[] offsets) where T : unmanaged;

    public bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryReadSpan<T>(out results, length, MainModule, baseOffset, offsets);
    }

    public bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            results = default;
            return false;
        }

        return TryReadSpan<T>(out results, length, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            results = default;
            return false;
        }

        return TryReadSpan<T>(out results, length, module.Base + baseOffset, offsets);
    }

    public bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, nint baseAddress, params int[] offsets) where T : unmanaged
    {
        ThrowHelper.ThrowIfLessThan(length, 0);

        results = new T[length];
        return TryReadSpan<T>(results, baseAddress, offsets);
    }

    public bool TryReadSpan<T>(Span<T> buffer, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryReadSpan<T>(buffer, MainModule, baseOffset, offsets);
    }

    public bool TryReadSpan<T>(Span<T> buffer, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return TryReadSpan<T>(buffer, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadSpan<T>(Span<T> buffer, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return TryReadSpan<T>(buffer, module.Base + baseOffset, offsets);
    }

    public abstract bool TryReadSpan<T>(Span<T> buffer, nint baseAddress, params int[] offsets) where T : unmanaged;
}
