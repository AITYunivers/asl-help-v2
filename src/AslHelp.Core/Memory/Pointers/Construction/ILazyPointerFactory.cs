using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Construction;

public interface ILazyPointerFactory
{
    ILazyPointerFactory Make<T>(int baseOffset, params int[] offsets);
    ILazyPointerFactory Make<T>(string module, int baseOffset, params int[] offsets);
    ILazyPointerFactory Make<T>(Module module, int baseOffset, params int[] offsets);
    ILazyPointerFactory Make<T>(nint baseAddress, params int[] offsets);

    ILazyPointerFactory MakeSpan<T>(int length, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeSpan<T>(int length, string module, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeSpan<T>(int length, Module module, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeSpan<T>(int length, nint baseAddress, params int[] offsets);

    ILazyPointerFactory MakeString(int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeString(int length, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeString(ReadStringType stringType, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeString(int length, ReadStringType stringType, int baseOffset, params int[] offsets);

    ILazyPointerFactory MakeString(string module, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeString(int length, string module, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeString(ReadStringType stringType, string module, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets);

    ILazyPointerFactory MakeString(Module module, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeString(int length, Module module, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets);

    ILazyPointerFactory MakeString(nint baseAddress, params int[] offsets);
    ILazyPointerFactory MakeString(int length, nint baseAddress, params int[] offsets);
    ILazyPointerFactory MakeString(ReadStringType stringType, nint baseAddress, params int[] offsets);
    ILazyPointerFactory MakeString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets);

    IParentStage MakeParent(int baseOffset, params int[] offsets);
    IParentStage MakeParent(string module, int baseOffset, params int[] offsets);
    IParentStage MakeParent(Module module, int baseOffset, params int[] offsets);
    IParentStage MakeParent(nint baseAddress, params int[] offsets);
}
