using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Construction;

public interface IPointerFactory
{
    Pointer<T> Make<T>(nint baseAddress, params int[] offsets) where T : unmanaged;
    Pointer<T> Make<T>(int baseOffset, params int[] offsets) where T : unmanaged;
    Pointer<T> Make<T>(string moduleName, int baseOffset, params int[] offsets) where T : unmanaged;
    Pointer<T> Make<T>(Module module, int baseOffset, params int[] offsets) where T : unmanaged;

    SpanPointer<T> MakeSpan<T>(int length, nint baseAddress, params int[] offsets) where T : unmanaged;
    SpanPointer<T> MakeSpan<T>(int length, int baseOffset, params int[] offsets) where T : unmanaged;
    SpanPointer<T> MakeSpan<T>(int length, string moduleName, int baseOffset, params int[] offsets) where T : unmanaged;
    SpanPointer<T> MakeSpan<T>(int length, Module module, int baseOffset, params int[] offsets) where T : unmanaged;

    StringPointer MakeString(nint baseAddress, params int[] offsets);
    StringPointer MakeString(int baseOffset, params int[] offsets);
    StringPointer MakeString(string moduleName, int baseOffset, params int[] offsets);
    StringPointer MakeString(Module module, int baseOffset, params int[] offsets);

    StringPointer MakeString(int length, nint baseAddress, params int[] offsets);
    StringPointer MakeString(int length, int baseOffset, params int[] offsets);
    StringPointer MakeString(int length, string moduleName, int baseOffset, params int[] offsets);
    StringPointer MakeString(int length, Module module, int baseOffset, params int[] offsets);

    StringPointer MakeString(ReadStringType stringType, nint baseAddress, params int[] offsets);
    StringPointer MakeString(ReadStringType stringType, int baseOffset, params int[] offsets);
    StringPointer MakeString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets);
    StringPointer MakeString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets);

    StringPointer MakeString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets);
    StringPointer MakeString(int length, ReadStringType stringType, int baseOffset, params int[] offsets);
    StringPointer MakeString(int length, ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets);
    StringPointer MakeString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets);

    SizedStringPointer MakeSizedString(nint baseAddress, params int[] offsets);
    SizedStringPointer MakeSizedString(int baseOffset, params int[] offsets);
    SizedStringPointer MakeSizedString(string moduleName, int baseOffset, params int[] offsets);
    SizedStringPointer MakeSizedString(Module module, int baseOffset, params int[] offsets);

    SizedStringPointer MakeSizedString(ReadStringType stringType, nint baseAddress, params int[] offsets);
    SizedStringPointer MakeSizedString(ReadStringType stringType, int baseOffset, params int[] offsets);
    SizedStringPointer MakeSizedString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets);
    SizedStringPointer MakeSizedString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets);
}
