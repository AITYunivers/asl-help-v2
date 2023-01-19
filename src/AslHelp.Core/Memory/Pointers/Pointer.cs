namespace AslHelp.Core.Memory.Pointers;

public sealed class Pointer<T> : PointerBase<T> where T : unmanaged
{
    public Pointer(IHelper helper, nint @base, params int[] offsets)
        : base(helper, @base, offsets) { }

    public Pointer(IHelper helper, PointerBase<nint> parent, int baseOffset, params int[] offsets)
        : base(helper, parent, baseOffset, offsets) { }

    protected override T Default { get; } = default;

    protected override bool TryUpdate(out T result)
    {
        return _helper.TryRead(out result, Address);
    }

    protected override bool CheckChanged(T old, T current)
    {
        return !old.Equals(current);
    }

    protected override bool Write(T value)
    {
        return _helper.Write(value, Address);
    }

    public override string ToString()
    {
        return $"Pointer<{typeof(T).Name}>({OffsetsToString()})";
    }
}
