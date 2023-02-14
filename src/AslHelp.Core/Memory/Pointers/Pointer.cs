using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers;

public sealed class Pointer<T> : PointerBase<T> where T : unmanaged
{
    public Pointer(IMemoryManager manager, nint @base, params int[] offsets)
        : base(manager, @base, offsets) { }

    public Pointer(IMemoryManager manager, PointerBase<nint> parent, int baseOffset, params int[] offsets)
        : base(manager, parent, baseOffset, offsets) { }

    protected override T Default { get; } = default;

    protected override bool TryUpdate(out T result)
    {
        return _manager.TryRead(out result, Address);
    }

    protected override bool CheckChanged(T old, T current)
    {
        return !old.Equals(current);
    }

    protected override bool Write(T value)
    {
        return _manager.Write(value, Address);
    }

    public override string ToString()
    {
        return $"Pointer<{typeof(T).Name}>({OffsetsToString()})";
    }
}
