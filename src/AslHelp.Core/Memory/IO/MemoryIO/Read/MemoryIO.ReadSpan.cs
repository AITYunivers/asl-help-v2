namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryIO
{
    public T[] ReadSpan<T>(int length, int baseOffset, params int[] offsets) where T : unmanaged
    {
        T[] results = new T[length];
        _ = TryReadSpan<T>(results, MainModule, baseOffset, offsets);

        return results;
    }

    public T[] ReadSpan<T>(int length, string module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        T[] results = new T[length];
        _ = TryReadSpan<T>(results, Modules[module], baseOffset, offsets);

        return results;
    }

    public T[] ReadSpan<T>(int length, Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        T[] results = new T[length];
        _ = TryReadSpan<T>(results, module, baseOffset, offsets);

        return results;
    }

    public T[] ReadSpan<T>(int length, nint baseAddress, params int[] offsets) where T : unmanaged
    {
        T[] results = new T[length];
        _ = TryReadSpan<T>(results, baseAddress, offsets);

        return results;
    }

    public bool TryReadSpan<T>(out T[] results, int length, int baseOffset, params int[] offsets) where T : unmanaged
    {
        results = new T[length];
        return TryReadSpan<T>(results, MainModule, baseOffset, offsets);
    }

    public bool TryReadSpan<T>(out T[] results, int length, string module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        results = new T[length];
        return TryReadSpan<T>(results, Modules[module], baseOffset, offsets);
    }

    public bool TryReadSpan<T>(out T[] results, int length, Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        results = new T[length];
        return TryReadSpan<T>(results, module, baseOffset, offsets);
    }

    public bool TryReadSpan<T>(out T[] results, int length, nint baseAddress, params int[] offsets) where T : unmanaged
    {
        results = new T[length];
        return TryReadSpan<T>(results, baseAddress, offsets);
    }

    public bool TryReadSpan<T>(Span<T> buffer, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryReadSpan<T>(buffer, MainModule, baseOffset, offsets);
    }

    public bool TryReadSpan<T>(Span<T> buffer, string module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryReadSpan<T>(buffer, Modules[module], baseOffset, offsets);
    }

    public bool TryReadSpan<T>(Span<T> buffer, Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            Debug.Warn($"[ReadSpan<{typeof(T).Name}>] Module could not be found.");

            return false;
        }

        return TryReadSpan<T>(buffer, module.Base + baseOffset, offsets);
    }

    public abstract bool TryReadSpan<T>(Span<T> buffer, nint baseAddress, params int[] offsets) where T : unmanaged;
}
