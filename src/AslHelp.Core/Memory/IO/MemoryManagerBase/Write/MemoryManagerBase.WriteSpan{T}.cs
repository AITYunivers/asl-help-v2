using AslHelp.Core.Extensions;
using AslHelp.Core.Reflection;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
{
    public bool WriteSpan<T>(ICollection<T> values, int baseOffset, params int[] offsets) where T : unmanaged
    {
        int count = values.Count;

        T[] rented = ArrayPoolExtensions.Rent<T>(count);
        values.CopyTo(rented, 0);

        bool success = WriteSpan<T>(rented.AsSpan(0, count), MainModule, baseOffset, offsets);

        ArrayPoolExtensions.Return(rented);

        return success;
    }

    public bool WriteSpan<T>(ICollection<T> values, string moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        int count = values.Count;

        T[] rented = ArrayPoolExtensions.Rent<T>(count);
        values.CopyTo(rented, 0);

        bool success = WriteSpan<T>(rented.AsSpan(0, count), Modules[moduleName], baseOffset, offsets);

        ArrayPoolExtensions.Return(rented);

        return success;
    }

    public bool WriteSpan<T>(ICollection<T> values, Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        int count = values.Count;

        T[] rented = ArrayPoolExtensions.Rent<T>(count);
        values.CopyTo(rented, 0);

        bool success = WriteSpan<T>(rented.AsSpan(0, count), module, baseOffset, offsets);

        ArrayPoolExtensions.Return(rented);

        return success;
    }

    public bool WriteSpan<T>(ICollection<T> values, nint baseAddress, params int[] offsets) where T : unmanaged
    {
        int count = values.Count;

        T[] rented = ArrayPoolExtensions.Rent<T>(count);
        values.CopyTo(rented, 0);

        bool success = WriteSpan<T>(rented.AsSpan(0, count), baseAddress, offsets);

        ArrayPoolExtensions.Return(rented);

        return success;
    }

    public bool WriteSpan<T>(List<T> values, int baseOffset, params int[] offsets) where T : unmanaged
    {
        (T[] items, int size) = Emissions<T>.GetBackingArray(values);
        return WriteSpan<T>(new ReadOnlySpan<T>(items, 0, size), MainModule, baseOffset, offsets);
    }

    public bool WriteSpan<T>(List<T> values, string moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        (T[] items, int size) = Emissions<T>.GetBackingArray(values);
        return WriteSpan<T>(new ReadOnlySpan<T>(items, 0, size), Modules[moduleName], baseOffset, offsets);
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

    public bool WriteSpan<T>(T[] values, string moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values.AsSpan(), Modules[moduleName], baseOffset, offsets);
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

    public bool WriteSpan<T>(ReadOnlySpan<T> values, string moduleName, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return WriteSpan<T>(values, Modules[moduleName], baseOffset, offsets);
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
