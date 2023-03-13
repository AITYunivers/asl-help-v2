using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Construction;

public interface IChildStage
{
    IChildOrNext MakeChild<T>(int baseOffset, params int[] offsets);
    IChildOrNext MakeChild<T>(string module, int baseOffset, params int[] offsets);
    IChildOrNext MakeChild<T>(Module module, int baseOffset, params int[] offsets);
    IChildOrNext MakeChild<T>(nint baseAddress, params int[] offsets);

    IChildOrNext MakeChildSpan<T>(int length, int baseOffset, params int[] offsets);
    IChildOrNext MakeChildSpan<T>(int length, string module, int baseOffset, params int[] offsets);
    IChildOrNext MakeChildSpan<T>(int length, Module module, int baseOffset, params int[] offsets);
    IChildOrNext MakeChildSpan<T>(int length, nint baseAddress, params int[] offsets);

    IChildOrNext MakeChildString(int baseOffset, params int[] offsets);
    IChildOrNext MakeChildString(int length, int baseOffset, params int[] offsets);
    IChildOrNext MakeChildString(ReadStringType stringType, int baseOffset, params int[] offsets);
    IChildOrNext MakeChildString(int length, ReadStringType stringType, int baseOffset, params int[] offsets);

    IChildOrNext MakeChildString(string module, int baseOffset, params int[] offsets);
    IChildOrNext MakeChildString(int length, string module, int baseOffset, params int[] offsets);
    IChildOrNext MakeChildString(ReadStringType stringType, string module, int baseOffset, params int[] offsets);
    IChildOrNext MakeChildString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets);

    IChildOrNext MakeChildString(Module module, int baseOffset, params int[] offsets);
    IChildOrNext MakeChildString(int length, Module module, int baseOffset, params int[] offsets);
    IChildOrNext MakeChildString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets);
    IChildOrNext MakeChildString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets);

    IChildOrNext MakeChildString(nint baseAddress, params int[] offsets);
    IChildOrNext MakeChildString(int length, nint baseAddress, params int[] offsets);
    IChildOrNext MakeChildString(ReadStringType stringType, nint baseAddress, params int[] offsets);
    IChildOrNext MakeChildString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets);
}

public interface IChildOrNext
    : ILazyPointerFactory,
    IChildStage
{ }
