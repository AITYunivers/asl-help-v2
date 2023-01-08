namespace System;

internal readonly struct Range : IEquatable<Range>
{
    public Range(Index start, Index end)
    {
        Start = start;
        End = end;
    }

    public Index Start { get; }
    public Index End { get; }

    public static Range All => new(Index.Start, Index.End);

    public static Range StartAt(Index start)
    {
        return new(start, Index.End);
    }

    public static Range EndAt(Index end)
    {
        return new(Index.Start, end);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (int Offset, int Length) GetOffsetAndLength(int length)
    {
        Index startIndex = Start;
        int start = startIndex.IsFromEnd ? length - startIndex.Value : startIndex.Value;

        Index endIndex = End;
        int end = endIndex.IsFromEnd ? length - endIndex.Value : endIndex.Value;

        if ((uint)end > (uint)length || (uint)start > (uint)end)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        return (start, end - start);
    }

    public bool Equals(Range other)
    {
        return other.Start.Equals(Start) && other.End.Equals(End);
    }

    public override bool Equals(object obj)
    {
        return obj is Range r && r.Start.Equals(Start) && r.End.Equals(End);
    }

    public override int GetHashCode()
    {
        int h1 = Start.GetHashCode(), h2 = End.GetHashCode();

        uint rol5 = ((uint)h1 << 5) | ((uint)h1 >> 27);
        return ((int)rol5 + h1) ^ h2;
    }

    public override string ToString()
    {
        return Start.ToString() + ".." + End.ToString();
    }
}
