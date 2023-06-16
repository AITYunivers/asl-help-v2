using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Core.Memory.Pointers;

public sealed class SpanPointer<T> : PointerBase<T[]>
    where T : unmanaged
{
    private readonly int _length;

    public SpanPointer(IMemoryManager manager, int length, nuint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets)
    {
        _length = length;
    }

    public SpanPointer(IMemoryManager manager, int length, IPointer<nint> parent, int nextOffset, params int[] remainingOffsets)
        : base(manager, parent, nextOffset, remainingOffsets)
    {
        _length = length;
    }

    protected override T[] Default { get; } = Array.Empty<T>();

    protected override bool TryUpdate(nuint address, [NotNullWhen(true)] out T[]? result)
    {
        return _manager.TryReadSpan(out result, _length, address);
    }

    protected override bool Write(nuint address, T[] value)
    {
        return _manager.WriteSpan(value, address);
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
        return $"{nameof(SpanPointer<T>)}<{typeof(T).Name}>({OffsetsToString()})";
    }
}
