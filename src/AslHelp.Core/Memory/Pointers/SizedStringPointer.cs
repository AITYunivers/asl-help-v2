using System;
using AslHelp.Core.Memory.IO;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers;

public sealed class SizedStringPointer : PointerBase<string>
{
    private readonly ReadStringType _stringType;

    public SizedStringPointer(IMemoryManager manager, ReadStringType stringType, nint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets)
    {
        _stringType = stringType;
    }

    public SizedStringPointer(IMemoryManager manager, ReadStringType stringType, IPointer<nint> parent, int nextOffset, params int[] offsets)
        : base(manager, parent, nextOffset, offsets)
    {
        _stringType = stringType;
    }

    protected override string Default { get; } = null;

    protected override bool TryUpdate(out string result)
    {
        return _manager.TryReadSizedString(out result, _stringType, Address);
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
