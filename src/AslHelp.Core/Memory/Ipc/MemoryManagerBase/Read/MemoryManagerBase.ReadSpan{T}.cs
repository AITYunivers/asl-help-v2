using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public T[] ReadSpan<T>(int length, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = "MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ReadSpan<T>(length, module, baseOffset, offsets);
    }

    public T[] ReadSpan<T>(int length, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ReadSpan<T>(length, module, baseOffset, offsets);
    }

    public T[] ReadSpan<T>(int length, Module module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return ReadSpan<T>(length, module.Base + baseOffset, offsets);
    }

    public T[] ReadSpan<T>(int length, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        ThrowHelper.ThrowIfLessThan(length, 0);

        T[] results = new T[length];
        ReadSpan<T>(results, baseAddress, offsets);

        return results;
    }

    public void ReadSpan<T>(Span<T> buffer, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = "MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        ReadSpan<T>(buffer, module, baseOffset, offsets);
    }

    public void ReadSpan<T>(Span<T> buffer, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        ReadSpan<T>(buffer, module, baseOffset, offsets);
    }

    public void ReadSpan<T>(Span<T> buffer, Module module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        ReadSpan<T>(buffer, module.Base + baseOffset, offsets);
    }

    public unsafe void ReadSpan<T>(Span<T> buffer, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        fixed (T* pBuffer = buffer)
        {
            Read(pBuffer, GetNativeSizeOf<T>(buffer.Length), baseAddress, offsets);
        }
    }

    public bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryReadSpan<T>(out results, length, MainModule, baseOffset, offsets);
    }

    public bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            results = default;
            return false;
        }

        return TryReadSpan<T>(out results, length, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            results = default;
            return false;
        }

        return TryReadSpan<T>(out results, length, module.Base + baseOffset, offsets);
    }

    public bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        ThrowHelper.ThrowIfLessThan(length, 0);

        results = new T[length];
        return TryReadSpan<T>(results, baseAddress, offsets);
    }

    public bool TryReadSpan<T>(Span<T> buffer, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryReadSpan<T>(buffer, MainModule, baseOffset, offsets);
    }

    public bool TryReadSpan<T>(Span<T> buffer, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return TryReadSpan<T>(buffer, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadSpan<T>(Span<T> buffer, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return TryReadSpan<T>(buffer, module.Base + baseOffset, offsets);
    }

    public unsafe bool TryReadSpan<T>(Span<T> buffer, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        fixed (T* pBuffer = buffer)
        {
            return TryRead<T>(pBuffer, GetNativeSizeOf<T>(buffer.Length), baseAddress, offsets);
        }
    }
}
