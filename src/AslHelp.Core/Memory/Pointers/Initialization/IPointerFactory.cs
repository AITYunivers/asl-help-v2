using System.Collections.Generic;

using AslHelp.Core.Reflection;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Initialization;

public interface IPointerFactory : IDictionary<string, IPointer>
{
    void MapTo(IDictionary<string, object> destination);

    Pointer<T> Make<T>(nuint baseAddress, params int[] offsets) where T : unmanaged;
    Pointer<T> Make<T>(uint baseOffset, params int[] offsets) where T : unmanaged;
    Pointer<T> Make<T>(string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    Pointer<T> Make<T>(Module module, uint baseOffset, params int[] offsets) where T : unmanaged;

    SpanPointer<T> MakeSpan<T>(int length, nuint baseAddress, params int[] offsets) where T : unmanaged;
    SpanPointer<T> MakeSpan<T>(int length, uint baseOffset, params int[] offsets) where T : unmanaged;
    SpanPointer<T> MakeSpan<T>(int length, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    SpanPointer<T> MakeSpan<T>(int length, Module module, uint baseOffset, params int[] offsets) where T : unmanaged;

    StringPointer MakeString(nuint baseAddress, params int[] offsets);
    StringPointer MakeString(uint baseOffset, params int[] offsets);
    StringPointer MakeString(string moduleName, uint baseOffset, params int[] offsets);
    StringPointer MakeString(Module module, uint baseOffset, params int[] offsets);

    StringPointer MakeString(int length, nuint baseAddress, params int[] offsets);
    StringPointer MakeString(int length, uint baseOffset, params int[] offsets);
    StringPointer MakeString(int length, string moduleName, uint baseOffset, params int[] offsets);
    StringPointer MakeString(int length, Module module, uint baseOffset, params int[] offsets);

    StringPointer MakeString(ReadStringType stringType, nuint baseAddress, params int[] offsets);
    StringPointer MakeString(ReadStringType stringType, uint baseOffset, params int[] offsets);
    StringPointer MakeString(ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets);
    StringPointer MakeString(ReadStringType stringType, Module module, uint baseOffset, params int[] offsets);

    StringPointer MakeString(int length, ReadStringType stringType, nuint baseAddress, params int[] offsets);
    StringPointer MakeString(int length, ReadStringType stringType, uint baseOffset, params int[] offsets);
    StringPointer MakeString(int length, ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets);
    StringPointer MakeString(int length, ReadStringType stringType, Module module, uint baseOffset, params int[] offsets);

    SizedStringPointer MakeSizedString(nuint baseAddress, params int[] offsets);
    SizedStringPointer MakeSizedString(uint baseOffset, params int[] offsets);
    SizedStringPointer MakeSizedString(string moduleName, uint baseOffset, params int[] offsets);
    SizedStringPointer MakeSizedString(Module module, uint baseOffset, params int[] offsets);

    SizedStringPointer MakeSizedString(ReadStringType stringType, nuint baseAddress, params int[] offsets);
    SizedStringPointer MakeSizedString(ReadStringType stringType, uint baseOffset, params int[] offsets);
    SizedStringPointer MakeSizedString(ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets);
    SizedStringPointer MakeSizedString(ReadStringType stringType, Module module, uint baseOffset, params int[] offsets);
}
