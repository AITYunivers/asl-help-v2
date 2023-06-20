using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Core.Memory.Pointers;

public sealed class Pointer<T> : PointerBase<T>
    where T : unmanaged
{
    public Pointer(IMemoryManager manager, nuint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets) { }

    public Pointer(IMemoryManager manager, IPointer<nuint> parent, int nextOffset, params int[] remainingOffsets)
        : base(manager, parent, nextOffset, remainingOffsets) { }

    protected override T Default { get; } = default;

    protected override bool TryUpdate([NotNullWhen(true)] out T result, nuint address)
    {
        return _manager.TryRead(out result, address);
    }

    protected override bool Write(T value, nuint address)
    {
        return _manager.TryWrite(value, address);
    }

    protected override bool HasChanged(T old, T current)
    {
        return !old.Equals(current);
    }

    public override string ToString()
    {
        return $"{nameof(Pointer<T>)}<{typeof(T).Name}>({OffsetsToString()})";
    }
}
