using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Memory.Pointers;
using AslHelp.Mono.Memory.Ipc;

namespace AslHelp.Mono.Memory.Pointers;

public sealed class ArrayPointer<T> : PointerBase<T[]>
    where T : unmanaged
{
    private readonly new IMonoMemoryManager _manager;

    public ArrayPointer(IMonoMemoryManager manager, nuint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets)
    {
        _manager = manager;
    }

    public ArrayPointer(IMonoMemoryManager manager, IPointer<nuint> parent, int nextOffset, params int[] remainingOffsets)
        : base(manager, parent, nextOffset, remainingOffsets)
    {
        _manager = manager;
    }

    protected override T[] Default { get; } = [];

    protected override bool TryUpdate([NotNullWhen(true)] out T[]? result, nuint address)
    {
        return _manager.TryReadArray(out result, address);
    }

    protected override bool Write(T[] value, nuint address)
    {
        return _manager.TryWriteArray(value, address);
    }

    protected override bool HasChanged(T[]? old, T[]? current)
    {
        if (old is null || current is null)
        {
            return old != current;
        }

        if (old.Length != current.Length)
        {
            return true;
        }

        for (var i = 0; i < old.Length; i++)
        {
            if (!old[i].Equals(current[i]))
            {
                return true;
            }
        }

        return false;
    }

    public override string ToString()
    {
        return $"{nameof(ArrayPointer<T>)}<{typeof(T).Name}>({OffsetsToString()})";
    }
}
