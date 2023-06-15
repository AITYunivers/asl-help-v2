using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AslHelp.Core.Reflection;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public bool WriteSpan<T>(ICollection<T> values, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values, MainModule, baseOffset, offsets);
    }

    public bool WriteSpan<T>(ICollection<T> values, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return WriteSpan<T>(values, Modules[moduleName], baseOffset, offsets);
    }

    public bool WriteSpan<T>(ICollection<T> values, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return WriteSpan<T>(values, module.Base + baseOffset, offsets);
    }

    public bool WriteSpan<T>(ICollection<T> values, nint baseAddress, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values.ToArray(), baseAddress, offsets);
    }

    public bool WriteSpan<T>(List<T> values, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values, MainModule, baseOffset, offsets);
    }

    public bool WriteSpan<T>(List<T> values, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return WriteSpan<T>(values, Modules[moduleName], baseOffset, offsets);
    }

    public bool WriteSpan<T>(List<T> values, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return WriteSpan<T>(values, module.Base + baseOffset, offsets);
    }

    public bool WriteSpan<T>(List<T> values, nint baseAddress, params int[] offsets) where T : unmanaged
    {
        (T[] array, int count) = Emissions<T>.GetBackingArray(values);

        return WriteSpan<T>(array.AsSpan(0, count), baseAddress, offsets);
    }

    public bool WriteSpan<T>(T[] values, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values, MainModule, baseOffset, offsets);
    }

    public bool WriteSpan<T>(T[] values, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return WriteSpan<T>(values, Modules[moduleName], baseOffset, offsets);
    }

    public bool WriteSpan<T>(T[] values, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return WriteSpan<T>(values, module.Base + baseOffset, offsets);
    }

    public bool WriteSpan<T>(T[] values, nint baseAddress, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values.AsSpan(), baseAddress, offsets);
    }

    public bool WriteSpan<T>(ReadOnlySpan<T> values, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values, MainModule, baseOffset, offsets);
    }

    public bool WriteSpan<T>(ReadOnlySpan<T> values, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return WriteSpan<T>(values, Modules[moduleName], baseOffset, offsets);
    }

    public bool WriteSpan<T>(ReadOnlySpan<T> values, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return WriteSpan<T>(values, module.Base + baseOffset, offsets);
    }

    public abstract bool WriteSpan<T>(ReadOnlySpan<T> values, nint baseAddress, params int[] offsets) where T : unmanaged;
}
