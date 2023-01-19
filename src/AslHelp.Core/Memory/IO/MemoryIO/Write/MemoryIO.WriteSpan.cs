using System.Buffers;
using AslHelp.Core.Reflection;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryIO
{
    public bool WriteSpan<T>(ICollection<T> values, int baseOffset, params int[] offsets) where T : unmanaged
    {
        int count = values.Count;

        T[] poolArray = ArrayPool<T>.Shared.Rent(count);
        values.CopyTo(poolArray, 0);

        bool success = WriteSpan<T>(poolArray.AsSpan(0, count), MainModule, baseOffset, offsets);

        ArrayPool<T>.Shared.Return(poolArray);

        return success;
    }

    public bool WriteSpan<T>(ICollection<T> values, string module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        int count = values.Count;

        T[] poolArray = ArrayPool<T>.Shared.Rent(count);
        values.CopyTo(poolArray, 0);

        bool success = WriteSpan<T>(poolArray.AsSpan(0, count), Modules[module], baseOffset, offsets);

        ArrayPool<T>.Shared.Return(poolArray);

        return success;
    }

    public bool WriteSpan<T>(ICollection<T> values, Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        int count = values.Count;

        T[] poolArray = ArrayPool<T>.Shared.Rent(count);
        values.CopyTo(poolArray, 0);

        bool success = WriteSpan<T>(poolArray.AsSpan(0, count), module, baseOffset, offsets);

        ArrayPool<T>.Shared.Return(poolArray);

        return success;
    }

    public bool WriteSpan<T>(ICollection<T> values, nint baseAddress, params int[] offsets) where T : unmanaged
    {
        int count = values.Count;

        T[] poolArray = ArrayPool<T>.Shared.Rent(count);
        values.CopyTo(poolArray, 0);

        bool success = WriteSpan<T>(poolArray.AsSpan(0, count), baseAddress, offsets);

        ArrayPool<T>.Shared.Return(poolArray);

        return success;
    }

    public bool WriteSpan<T>(List<T> values, int baseOffset, params int[] offsets) where T : unmanaged
    {
        (T[] items, int size) = Emissions<T>.GetBackingArray(values);
        return WriteSpan<T>(new ReadOnlySpan<T>(items, 0, size), MainModule, baseOffset, offsets);
    }

    public bool WriteSpan<T>(List<T> values, string module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        (T[] items, int size) = Emissions<T>.GetBackingArray(values);
        return WriteSpan<T>(new ReadOnlySpan<T>(items, 0, size), Modules[module], baseOffset, offsets);
    }

    public bool WriteSpan<T>(List<T> values, Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        (T[] items, int size) = Emissions<T>.GetBackingArray(values);
        return WriteSpan<T>(new ReadOnlySpan<T>(items, 0, size), module, baseOffset, offsets);
    }

    public bool WriteSpan<T>(List<T> values, nint baseAddress, params int[] offsets) where T : unmanaged
    {
        (T[] items, int size) = Emissions<T>.GetBackingArray(values);
        return WriteSpan<T>(new ReadOnlySpan<T>(items, 0, size), baseAddress, offsets);
    }

    public bool WriteSpan<T>(T[] values, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values.AsSpan(), MainModule, baseOffset, offsets);
    }

    public bool WriteSpan<T>(T[] values, string module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values.AsSpan(), Modules[module], baseOffset, offsets);
    }

    public bool WriteSpan<T>(T[] values, Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values.AsSpan(), module, baseOffset, offsets);
    }

    public bool WriteSpan<T>(T[] values, nint baseAddress, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values.AsSpan(), baseAddress, offsets);
    }

    public bool WriteSpan<T>(ReadOnlySpan<T> values, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values, MainModule, baseOffset, offsets);
    }

    public bool WriteSpan<T>(ReadOnlySpan<T> values, string module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values, Modules[module], baseOffset, offsets);
    }

    public bool WriteSpan<T>(ReadOnlySpan<T> values, Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            Debug.Warn($"[WriteSpan<{typeof(T).Name}>] Module could not be found.");

            return false;
        }

        return WriteSpan<T>(values, module.Base + baseOffset, offsets);
    }

    public abstract bool WriteSpan<T>(ReadOnlySpan<T> values, nint baseAddress, params int[] offsets) where T : unmanaged;
}
