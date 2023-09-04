using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers;

public interface IParentPointer
{
    Pointer<TChild> Make<TChild>(int next, params int[] remainingOffsets) where TChild : unmanaged;

    SpanPointer<TChild> MakeSpan<TChild>(int length, int next, params int[] remainingOffsets) where TChild : unmanaged;

    StringPointer MakeString(int next, params int[] remainingOffsets);
    StringPointer MakeString(int length, int next, params int[] remainingOffsets);
    StringPointer MakeString(ReadStringType stringType, int next, params int[] remainingOffsets);
    StringPointer MakeString(int length, ReadStringType stringType, int next, params int[] remainingOffsets);

    SizedStringPointer MakeSizedString(int next, params int[] remainingOffsets);
    SizedStringPointer MakeSizedString(ReadStringType stringType, int next, params int[] remainingOffsets);
}
