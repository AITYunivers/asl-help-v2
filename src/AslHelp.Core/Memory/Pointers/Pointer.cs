using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using AslHelp.Common.Memory;
using AslHelp.Core.Memory.Ipc;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers;

public sealed partial class Pointer<T> : PointerBase<T>
    where T : unmanaged
{
    public Pointer(IMemoryManager manager, nuint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets) { }

    public Pointer(IMemoryManager manager, IPointer<nuint> parent, int nextOffset, params int[] remainingOffsets)
        : base(manager, parent, nextOffset, remainingOffsets) { }

    protected override T Default { get; } = default;

    protected override bool TryUpdate([NotNullWhen(true)] out T result, nuint address)
    {
        return _manager.TryRead(out result, address);
    }

    protected override bool Write(T value, nuint address)
    {
        return _manager.TryWrite(value, address);
    }

    protected override bool HasChanged(T old, T current)
    {
        return !old.Equals(current);
    }

    public override string ToString()
    {
        return $"{nameof(Pointer<T>)}<{typeof(T).Name}>({OffsetsToString()})";
    }
}

public partial class Pointer<T> : IParentPointer
{
    public Pointer<TChild> Make<TChild>(int nextOffset, params int[] remainingOffsets)
        where TChild : unmanaged
    {
        IPointer<nuint> parent = EnsureParentType();

        return new(_manager, parent, nextOffset, remainingOffsets);
    }

    public SpanPointer<TChild> MakeSpan<TChild>(int length, int nextOffset, params int[] remainingOffsets)
        where TChild : unmanaged
    {
        IPointer<nuint> parent = EnsureParentType();

        return new(_manager, length, parent, nextOffset, remainingOffsets);
    }

    public StringPointer MakeString(int nextOffset, params int[] remainingOffsets)
    {
        IPointer<nuint> parent = EnsureParentType();

        return new(_manager, IOR.DefaultStringReadLength, ReadStringType.AutoDetect, parent, nextOffset, remainingOffsets);
    }

    public StringPointer MakeString(int length, int nextOffset, params int[] remainingOffsets)
    {
        IPointer<nuint> parent = EnsureParentType();

        return new(_manager, length, ReadStringType.AutoDetect, parent, nextOffset, remainingOffsets);
    }

    public StringPointer MakeString(ReadStringType stringType, int nextOffset, params int[] remainingOffsets)
    {
        IPointer<nuint> parent = EnsureParentType();

        return new(_manager, IOR.DefaultStringReadLength, stringType, parent, nextOffset, remainingOffsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, int nextOffset, params int[] remainingOffsets)
    {
        IPointer<nuint> parent = EnsureParentType();

        return new(_manager, length, stringType, parent, nextOffset, remainingOffsets);
    }

    public SizedStringPointer MakeSizedString(int nextOffset, params int[] remainingOffsets)
    {
        IPointer<nuint> parent = EnsureParentType();

        return new(_manager, ReadStringType.AutoDetect, parent, nextOffset, remainingOffsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, int nextOffset, params int[] remainingOffsets)
    {
        IPointer<nuint> parent = EnsureParentType();

        return new(_manager, stringType, parent, nextOffset, remainingOffsets);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IPointer<nuint> EnsureParentType()
    {
        if (this is IPointer<nuint> parent)
        {
            return parent;
        }
        else
        {
            const string msg = $"Cannot create children from a non-IPointer<nuint> parent.";
            throw new InvalidOperationException(msg);
        }
    }
}
