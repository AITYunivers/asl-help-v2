using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

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
        if (!Is64Bit && IsNativeInt<T>())
        {
            Span<uint> buf32 = MemoryMarshal.Cast<T, uint>(buffer);
            Span<ulong> buf64 = MemoryMarshal.Cast<T, ulong>(buffer);

            ReadSpan<uint>(buf32[buf64.Length..], baseAddress, offsets);

            for (int i = 0; i < buf64.Length; i++)
            {
                buf64[i] = buf32[buf64.Length + i];
            }
        }

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
        if (!Is64Bit && IsNativeInt<T>())
        {
            Span<uint> buf32 = MemoryMarshal.Cast<T, uint>(buffer);
            Span<ulong> buf64 = MemoryMarshal.Cast<T, ulong>(buffer);

            if (!TryReadSpan<uint>(buf32[buf64.Length..], baseAddress, offsets))
            {
                return false;
            }

            for (int i = 0; i < buf64.Length; i++)
            {
                buf64[i] = buf32[buf64.Length + i];
            }

            return true;
        }

        fixed (T* pBuffer = buffer)
        {
            Result readResult = TryRead<T>(pBuffer, GetNativeSizeOf<T>(buffer.Length), baseAddress, offsets);
            return readResult.IsSuccess;
        }
    }
}
