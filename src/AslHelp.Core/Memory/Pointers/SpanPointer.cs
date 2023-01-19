namespace AslHelp.Core.Memory.Pointers;

public sealed class SpanPointer<T> : PointerBase<T[]> where T : unmanaged
{
    private readonly int _length;

    public SpanPointer(IMemoryManager manager, int length, nint @base, params int[] offsets)
        : base(manager, @base, offsets)
    {
        _length = length;
    }

    public SpanPointer(IMemoryManager manager, int length, PointerBase<nint> parent, int baseOffset, params int[] offsets)
        : base(manager, parent, baseOffset, offsets)
    {
        _length = length;
    }

    protected override T[] Default { get; } = Array.Empty<T>();

    protected override bool TryUpdate(out T[] result)
    {
        return _manager.TryReadSpan(out result, _length, Address);
    }

    protected override bool CheckChanged(T[] old, T[] current)
    {
        for (int i = 0; i < _length; i++)
        {
            if (!old[i].Equals(current[i]))
            {
                return false;
            }
        }

        return true;
    }

    protected override bool Write(T[] value)
    {
        return _manager.WriteSpan(value, Address);
    }

    public override string ToString()
    {
        return $"SpanPointer<{typeof(T).Name}>({OffsetsToString()})";
    }
}
