using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Results;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public T[] ReadSpan<T>(int length, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return ReadSpan<T>(length, MainModule.Base + baseOffset, offsets);
    }

    public T[] ReadSpan<T>(int length, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return ReadSpan<T>(length, Modules[moduleName].Base + baseOffset, offsets);
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
        ReadSpan<T>(buffer, MainModule.Base + baseOffset, offsets);
    }

    public void ReadSpan<T>(Span<T> buffer, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        ReadSpan<T>(buffer, Modules[moduleName].Base + baseOffset, offsets);
    }

    public void ReadSpan<T>(Span<T> buffer, Module module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        ReadSpan<T>(buffer, module.Base + baseOffset, offsets);
    }

    public unsafe void ReadSpan<T>(Span<T> buffer, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        fixed (T* pBuffer = buffer)
        {
            Result readResult = TryRead<T>(pBuffer, GetNativeSizeOf<T>(buffer.Length), baseAddress, offsets);

            if (!readResult.IsSuccess)
            {
                readResult.Throw();
            }
        }
    }

    public bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryReadSpan<T>(out results, length, MainModule.Base + baseOffset, offsets);
    }

    public bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            results = default;
            return false;
        }

        return TryReadSpan<T>(out results, length, Modules[moduleName].Base + baseOffset, offsets);
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
        if (length < 0)
        {
            results = default;
            return false;
        }

        results = new T[length];
        return TryReadSpan<T>(results, baseAddress, offsets);
    }

    public bool TryReadSpan<T>(Span<T> buffer, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryReadSpan<T>(buffer, MainModule.Base + baseOffset, offsets);
    }

    public bool TryReadSpan<T>(Span<T> buffer, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return TryReadSpan<T>(buffer, Modules[moduleName].Base + baseOffset, offsets);
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
            Result readResult = TryRead<T>(pBuffer, GetNativeSizeOf<T>(buffer.Length), baseAddress, offsets);
            return readResult.IsSuccess;
        }
    }
}
