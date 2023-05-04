using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers;

public sealed partial class Pointer<T>
{
    public Pointer<TChild> Make<TChild>(int nextOffset, params int[] remainingOffsets)
        where TChild : unmanaged
    {
        IPointer<nint> parent = EnsureParentType();

        return new(_manager, parent, nextOffset, remainingOffsets);
    }

    public SpanPointer<TChild> MakeSpan<TChild>(int length, int nextOffset, params int[] remainingOffsets)
        where TChild : unmanaged
    {
        IPointer<nint> parent = EnsureParentType();

        return new(_manager, length, parent, nextOffset, remainingOffsets);
    }

    public StringPointer MakeString(int nextOffset, params int[] remainingOffsets)
    {
        IPointer<nint> parent = EnsureParentType();

        return new(_manager, AHR.MaxStringReadLength, ReadStringType.AutoDetect, parent, nextOffset, remainingOffsets);
    }

    public StringPointer MakeString(int length, int nextOffset, params int[] remainingOffsets)
    {
        IPointer<nint> parent = EnsureParentType();

        return new(_manager, length, ReadStringType.AutoDetect, parent, nextOffset, remainingOffsets);
    }

    public StringPointer MakeString(ReadStringType stringType, int nextOffset, params int[] remainingOffsets)
    {
        IPointer<nint> parent = EnsureParentType();

        return new(_manager, AHR.MaxStringReadLength, stringType, parent, nextOffset, remainingOffsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, int nextOffset, params int[] remainingOffsets)
    {
        IPointer<nint> parent = EnsureParentType();

        return new(_manager, length, stringType, parent, nextOffset, remainingOffsets);
    }

    public SizedStringPointer MakeSizedString(int nextOffset, params int[] remainingOffsets)
    {
        IPointer<nint> parent = EnsureParentType();

        return new(_manager, ReadStringType.AutoDetect, parent, nextOffset, remainingOffsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, int nextOffset, params int[] remainingOffsets)
    {
        IPointer<nint> parent = EnsureParentType();

        return new(_manager, stringType, parent, nextOffset, remainingOffsets);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IPointer<nint> EnsureParentType()
    {
        if (this is IPointer<nint> parent)
        {
            return parent;
        }
        else
        {
            string msg = $"Cannot create children from a non-{nameof(IPointer<nint>)} parent.";
            throw new InvalidOperationException(msg);
        }
    }
}
