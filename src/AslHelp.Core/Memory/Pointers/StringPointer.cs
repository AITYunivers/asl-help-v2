﻿using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers;

public class StringPointer : PointerBase<string>
{
    private readonly int _length;
    private readonly ReadStringType _stringType;

    public StringPointer(IHelper helper, int length, ReadStringType stringType, nint @base, params int[] offsets)
        : base(helper, @base, offsets)
    {
        _length = length;
        _stringType = stringType;
    }

    public StringPointer(IHelper helper, int length, ReadStringType stringType, PointerBase<nint> parent, int baseOffset, params int[] offsets)
        : base(helper, parent, baseOffset, offsets)
    {
        _length = length;
        _stringType = stringType;
    }

    protected override string Default { get; } = null;

    protected override bool TryUpdate(out string result)
    {
        return _helper.TryReadString(out result, _length, _stringType, Address);
    }

    protected override bool CheckChanged(string old, string current)
    {
        return old != current;
    }

    protected override bool Write(string value)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"StringPointer({OffsetsToString()})";
    }
}
