using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers;

public sealed class Pointer<T>
    : PointerBase<T>
    where T : unmanaged
{
    public Pointer(IMemoryManager manager, nint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets) { }

    public Pointer(IMemoryManager manager, IPointer<nint> parent, int nextOffset, params int[] offsets)
        : base(manager, parent, nextOffset, offsets) { }

    protected override T Default { get; } = default;

    protected override bool TryUpdate(out T result)
    {
        return _manager.TryRead(out result, Address);
    }

    protected override bool CheckChanged(T old, T current)
    {
        return !old.Equals(current);
    }

    public override bool Write(T value)
    {
        return _manager.Write(value, Address);
    }

    public override string ToString()
    {
        return $"Pointer<{typeof(T).Name}>({OffsetsToString()})";
    }
}
