using System;
using AslHelp.Core.Memory.IO;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers;

public sealed class StringPointer : PointerBase<string>
{
    private readonly int _length;
    private readonly ReadStringType _stringType;

    public StringPointer(IMemoryManager manager, int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets)
    {
        _length = length;
        _stringType = stringType;
    }

    public StringPointer(IMemoryManager manager, int length, ReadStringType stringType, IPointer<nint> parent, int nextOffset, params int[] offsets)
        : base(manager, parent, nextOffset, offsets)
    {
        _length = length;
        _stringType = stringType;
    }

    protected override string Default { get; } = null;

    protected override bool TryUpdate(out string result)
    {
        return _manager.TryReadString(out result, _length, _stringType, DerefOffsets());
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
