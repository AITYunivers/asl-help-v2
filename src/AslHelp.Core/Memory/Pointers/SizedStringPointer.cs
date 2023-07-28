using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Memory.Ipc;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers;

public sealed class SizedStringPointer : PointerBase<string>
{
    private readonly ReadStringType _stringType;

    public SizedStringPointer(IMemoryManager manager, nuint baseAddress, params int[] offsets)
        : this(manager, ReadStringType.AutoDetect, baseAddress, offsets) { }

    public SizedStringPointer(IMemoryManager manager, ReadStringType stringType, nuint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets)
    {
        _stringType = stringType;
    }

    public SizedStringPointer(IMemoryManager manager, IPointer<nuint> parent, int nextOffset, params int[] remainingOffsets)
        : this(manager, ReadStringType.AutoDetect, parent, nextOffset, remainingOffsets) { }

    public SizedStringPointer(
        IMemoryManager manager,
        ReadStringType stringType,
        IPointer<nuint> parent,
        int nextOffset,
        params int[] remainingOffsets)
        : base(manager, parent, nextOffset, remainingOffsets)
    {
        _stringType = stringType;
    }

    protected override string? Default { get; } = null;

    protected override bool TryUpdate([NotNullWhen(true)] out string? result, nuint address)
    {
        return _manager.TryReadSizedString(out result, _stringType, address);
    }

    protected override bool Write(string value, nuint address)
    {
        throw new NotImplementedException();
    }

    protected override bool HasChanged(string? old, string? current)
    {
        return old != current;
    }

    public override string ToString()
    {
        return $"{nameof(SizedStringPointer)}<{typeof(string).Name}>({OffsetsToString()})";
    }
}
