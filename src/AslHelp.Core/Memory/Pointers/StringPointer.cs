using AslHelp.Core.Memory.IO;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers;

public sealed class StringPointer
    : PointerBase<string>
{
    private readonly bool _sized;
    private readonly int _length;
    private readonly ReadStringType _stringType;

    public StringPointer(IMemoryManager manager, int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets)
    {
        _sized = false;
        _length = length;
        _stringType = stringType;
    }

    public StringPointer(IMemoryManager manager, int length, ReadStringType stringType, IPointer<nint> parent, int nextOffset, params int[] offsets)
        : base(manager, parent, nextOffset, offsets)
    {
        _sized = false;
        _length = length;
        _stringType = stringType;
    }

    public StringPointer(IMemoryManager manager, ReadStringType stringType, nint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets)
    {
        _sized = true;
        _stringType = stringType;
    }

    public StringPointer(IMemoryManager manager, ReadStringType stringType, IPointer<nint> parent, int nextOffset, params int[] offsets)
        : base(manager, parent, nextOffset, offsets)
    {
        _sized = true;
        _stringType = stringType;
    }

    protected override string Default { get; } = null;

    protected override bool TryUpdate(out string result)
    {
        return _sized
            ? _manager.TryReadSizedString(out result, _stringType, Address)
            : _manager.TryReadString(out result, _length, _stringType, Address);
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
