using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Memory.Pointers;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.Pointers;

public sealed class ListPointer<T> : PointerBase<List<T>>
    where T : unmanaged
{
    private readonly new IMonoMemoryManager _manager;

    public ListPointer(IMonoMemoryManager manager, nuint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets)
    {
        _manager = manager;
    }

    public ListPointer(IMonoMemoryManager manager, IPointer<nuint> parent, int nextOffset, params int[] remainingOffsets)
        : base(manager, parent, nextOffset, remainingOffsets)
    {
        _manager = manager;
    }

    protected override List<T> Default => [];

    protected override bool TryUpdate([NotNullWhen(true)] out List<T>? result, nuint address)
    {
        return _manager.TryReadList(out result, address);
    }

    protected override bool Write(List<T> value, nuint address)
    {
        return _manager.TryWriteList(value, address);
    }

    protected override bool HasChanged(List<T>? old, List<T>? current)
    {
        if (old is null || current is null)
        {
            return old != current;
        }

        if (old.Count != current.Count)
        {
            return true;
        }

        for (var i = 0; i < old.Count; i++)
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
        return $"{nameof(ListPointer<T>)}<{typeof(T).Name}>({OffsetsToString()})";
    }
}
