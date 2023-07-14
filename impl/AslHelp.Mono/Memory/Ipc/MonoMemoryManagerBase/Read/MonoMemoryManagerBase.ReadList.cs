using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using AslHelp.Core.Memory;
using AslHelp.Core.Reflection;

namespace AslHelp.Unity.Memory.Ipc;

public partial class MonoMemoryManagerBase
{
    public List<T> ReadList<T>(uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return ReadList<T>(MainModule.Base + baseOffset, offsets);
    }

    public List<T> ReadList<T>(string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return ReadList<T>(Modules[moduleName].Base + baseOffset, offsets);
    }

    public List<T> ReadList<T>(Module module, uint baseOffset, int[] offsets) where T : unmanaged
    {
        return ReadList<T>(module.Base + baseOffset, offsets);
    }

    public List<T> ReadList<T>(nuint address, params int[] offsets) where T : unmanaged
    {
        nuint deref = Read<nuint>(address, offsets);

        int count = Read<int>(deref + (uint)(PtrSize * 3));

        List<T> results = new(count);

        (T[] items, _) = Emissions<T>.GetBackingArray(results);
        Span<T> buffer = new(items, 0, count);

        ReadSpan<T>(buffer, deref + (nuint)(PtrSize * 2), PtrSize * 4);

        return results;
    }

    public bool TryReadList<T>([NotNullWhen(true)] out List<T>? results, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryReadList<T>(out results, MainModule.Base + baseOffset, offsets);
    }

    public bool TryReadList<T>([NotNullWhen(true)] out List<T>? results, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            results = default;
            return false;
        }

        return TryReadList<T>(out results, Modules[moduleName].Base + baseOffset, offsets);
    }

    public bool TryReadList<T>([NotNullWhen(true)] out List<T>? results, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            results = default;
            return false;
        }

        return TryReadList<T>(out results, module.Base + baseOffset, offsets);
    }

    public unsafe bool TryReadList<T>([NotNullWhen(true)] out List<T>? results, nuint address, params int[] offsets) where T : unmanaged
    {
        if (!TryRead(out nuint deref, address, offsets))
        {
            results = default;
            return false;
        }

        if (!TryRead(out int count, deref + (uint)(PtrSize * 3)))
        {
            results = default;
            return false;
        }

        results = new(count);

        (T[] items, _) = Emissions<T>.GetBackingArray(results);
        Span<T> buffer = new(items, 0, count);

        if (!TryReadSpan(buffer, deref + (nuint)(PtrSize * 2), PtrSize * 4))
        {
            results = default;
            return false;
        }

        return true;
    }
}
