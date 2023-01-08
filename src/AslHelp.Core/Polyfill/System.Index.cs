namespace System;

internal readonly struct Index : IEquatable<Index>
{
    private readonly int _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Index(int value, bool fromEnd = false)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException("Non-negative number required.");
        }

        _value = fromEnd ? ~value : value;
    }

    private Index(int value)
    {
        _value = value;
    }

    public int Value => _value < 0 ? ~_value : _value;
    public bool IsFromEnd => _value < 0;

    public static Index Start => new(0);
    public static Index End => new(~0);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Index FromStart(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException("Non-negative number required.");
        }

        return new(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Index FromEnd(int value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException("Non-negative number required.");
        }

        return new Index(~value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public int GetOffset(int length)
    {
        int offset = _value;
        if (IsFromEnd)
        {
            // offset = length - (~value)
            // offset = length + (~(~value) + 1)
            // offset = length + value + 1

            offset += length + 1;
        }

        return offset;
    }

    public static implicit operator Index(int value)
    {
        return FromStart(value);
    }

    public bool Equals(Index other)
    {
        return _value == other._value;
    }

    public override bool Equals(object obj)
    {
        return obj is Index i && _value == i._value;
    }

    public override int GetHashCode()
    {
        return _value;
    }

    public override string ToString()
    {
        if (IsFromEnd)
        {
            return '^' + Value.ToString();
        }

        return ((uint)Value).ToString();
    }
}
