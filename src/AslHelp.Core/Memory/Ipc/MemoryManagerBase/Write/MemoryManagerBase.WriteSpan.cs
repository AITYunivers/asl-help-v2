using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using AslHelp.Common.Results;
using AslHelp.Core.Reflection;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public void WriteSpan<T>(ICollection<T> values, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values, MainModule.Base + baseOffset, offsets);
    }

    public void WriteSpan<T>(ICollection<T> values, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values, Modules[moduleName].Base + baseOffset, offsets);
    }

    public void WriteSpan<T>(ICollection<T> values, Module module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values, module.Base + baseOffset, offsets);
    }

    public void WriteSpan<T>(ICollection<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values.ToArray(), baseAddress, offsets);
    }

    public bool TryWriteSpan<T>(ICollection<T> values, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryWriteSpan<T>(values, MainModule.Base + baseOffset, offsets);
    }

    public bool TryWriteSpan<T>(ICollection<T> values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return TryWriteSpan<T>(values, Modules[moduleName].Base + baseOffset, offsets);
    }

    public bool TryWriteSpan<T>(ICollection<T> values, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return TryWriteSpan<T>(values, module.Base + baseOffset, offsets);
    }

    public bool TryWriteSpan<T>(ICollection<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        return TryWriteSpan<T>(values.ToArray(), baseAddress, offsets);
    }

    public void WriteSpan<T>(List<T> values, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values, MainModule.Base + baseOffset, offsets);
    }

    public void WriteSpan<T>(List<T> values, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values, Modules[moduleName].Base + baseOffset, offsets);
    }

    public void WriteSpan<T>(List<T> values, Module module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values, module.Base + baseOffset, offsets);
    }

    public void WriteSpan<T>(List<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        (T[] array, int count) = Emissions<T>.GetBackingArray(values);
        WriteSpan<T>(array.AsSpan(0, count), baseAddress, offsets);
    }

    public bool TryWriteSpan<T>(List<T> values, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryWriteSpan<T>(values, MainModule.Base + baseOffset, offsets);
    }

    public bool TryWriteSpan<T>(List<T> values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return TryWriteSpan<T>(values, Modules[moduleName].Base + baseOffset, offsets);
    }

    public bool TryWriteSpan<T>(List<T> values, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return TryWriteSpan<T>(values, module.Base + baseOffset, offsets);
    }

    public bool TryWriteSpan<T>(List<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        (T[] array, int count) = Emissions<T>.GetBackingArray(values);
        return TryWriteSpan<T>(array.AsSpan(0, count), baseAddress, offsets);
    }

    public void WriteSpan<T>(T[] values, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values, MainModule.Base + baseOffset, offsets);
    }

    public void WriteSpan<T>(T[] values, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values, Modules[moduleName].Base + baseOffset, offsets);
    }

    public void WriteSpan<T>(T[] values, Module module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values, module.Base + baseOffset, offsets);
    }

    public void WriteSpan<T>(T[] values, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values.AsSpan(), baseAddress, offsets);
    }

    public void WriteSpan<T>(ReadOnlySpan<T> values, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values, MainModule.Base + baseOffset, offsets);
    }

    public void WriteSpan<T>(ReadOnlySpan<T> values, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values, Modules[moduleName].Base + baseOffset, offsets);
    }

    public void WriteSpan<T>(ReadOnlySpan<T> values, Module module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        WriteSpan<T>(values, module.Base + baseOffset, offsets);
    }

    public unsafe void WriteSpan<T>(ReadOnlySpan<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        fixed (T* pValues = values)
        {
            Result writeResult = TryWrite<T>(pValues, GetNativeSizeOf<T>(values.Length), baseAddress, offsets);
            if (!writeResult.IsSuccess)
            {
                writeResult.Throw();
            }
        }
    }

    public bool TryWriteSpan<T>(T[] values, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryWriteSpan<T>(values, MainModule.Base + baseOffset, offsets);
    }

    public bool TryWriteSpan<T>(T[] values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return TryWriteSpan<T>(values, Modules[moduleName].Base + baseOffset, offsets);
    }

    public bool TryWriteSpan<T>(T[] values, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return TryWriteSpan<T>(values, module.Base + baseOffset, offsets);
    }

    public bool TryWriteSpan<T>(T[] values, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        return TryWriteSpan<T>(values.AsSpan(), baseAddress, offsets);
    }

    public bool TryWriteSpan<T>(ReadOnlySpan<T> values, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryWriteSpan<T>(values, MainModule.Base + baseOffset, offsets);
    }

    public bool TryWriteSpan<T>(ReadOnlySpan<T> values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return TryWriteSpan<T>(values, Modules[moduleName].Base + baseOffset, offsets);
    }

    public bool TryWriteSpan<T>(ReadOnlySpan<T> values, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return TryWriteSpan<T>(values, module.Base + baseOffset, offsets);
    }

    public unsafe bool TryWriteSpan<T>(ReadOnlySpan<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        fixed (T* pValues = values)
        {
            Result writeResult = TryWrite<T>(pValues, GetNativeSizeOf<T>(values.Length), baseAddress, offsets);
            return writeResult.IsSuccess;
        }
    }
}
