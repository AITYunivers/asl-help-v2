using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Memory.Ipc;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers;

public sealed class StringPointer : PointerBase<string>
{
    private readonly ReadStringType _stringType;
    private readonly int _length;

    public StringPointer(IMemoryManager manager, int length, ReadStringType stringType, nuint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets)
    {
        _stringType = stringType;
        _length = length;
    }

    public StringPointer(
        IMemoryManager manager,
        int length,
        ReadStringType stringType,
        IPointer<nint> parent,
        int nextOffset,
        params int[] remainingOffsets)
        : base(manager, parent, nextOffset, remainingOffsets)
    {
        _stringType = stringType;
        _length = length;
    }

    protected override string? Default { get; }

    protected override bool TryUpdate(nuint address, [NotNullWhen(true)] out string? result)
    {
        return _manager.TryReadSizedString(out result, address);
    }

    protected override bool Write(nuint address, string value)
    {
        throw new NotImplementedException();
    }

    protected override bool HasChanged(string? old, string? current)
    {
        return old != current;
    }

    public override string ToString()
    {
        return $"{nameof(StringPointer)}<{typeof(string).Name}>({OffsetsToString()})";
    }
}
