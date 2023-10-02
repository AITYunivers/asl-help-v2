using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers;

public interface IParentPointer
{
    Pointer<TChild> Make<TChild>(int nextOffset, params int[] remainingOffsets) where TChild : unmanaged;

    SpanPointer<TChild> MakeSpan<TChild>(int length, int nextOffset, params int[] remainingOffsets) where TChild : unmanaged;

    StringPointer MakeString(int nextOffset, params int[] remainingOffsets);
    StringPointer MakeString(int length, int nextOffset, params int[] remainingOffsets);
    StringPointer MakeString(ReadStringType stringType, int nextOffset, params int[] remainingOffsets);
    StringPointer MakeString(int length, ReadStringType stringType, int nextOffset, params int[] remainingOffsets);

    SizedStringPointer MakeSizedString(int nextOffset, params int[] remainingOffsets);
    SizedStringPointer MakeSizedString(ReadStringType stringType, int nextOffset, params int[] remainingOffsets);
}
