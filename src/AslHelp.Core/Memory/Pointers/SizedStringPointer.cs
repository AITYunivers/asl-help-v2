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

    public SizedStringPointer(IMemoryManager manager, IPointer<nint> parent, int nextOffset, params int[] remainingOffsets)
        : this(manager, ReadStringType.AutoDetect, parent, nextOffset, remainingOffsets) { }

    public SizedStringPointer(
        IMemoryManager manager,
        ReadStringType stringType,
        IPointer<nint> parent,
        int nextOffset,
        params int[] remainingOffsets)
        : base(manager, parent, nextOffset, remainingOffsets)
    {
        _stringType = stringType;
    }

    protected override string? Default { get; }

    protected override bool TryUpdate(nuint address, [NotNullWhen(true)] out string? result)
    {
        return _manager.TryReadSizedString(out result, _stringType, address);
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
        return $"{nameof(SizedStringPointer)}<{typeof(string).Name}>({OffsetsToString()})";
    }
}
