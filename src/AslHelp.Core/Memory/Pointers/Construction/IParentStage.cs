using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Construction;

public interface IParentStage
{
    IChildStage MakeChild<T>(int baseOffset, params int[] offsets);
    IChildStage MakeChild<T>(string module, int baseOffset, params int[] offsets);
    IChildStage MakeChild<T>(Module module, int baseOffset, params int[] offsets);
    IChildStage MakeChild<T>(nint baseAddress, params int[] offsets);

    IChildStage MakeChildSpan<T>(int length, int baseOffset, params int[] offsets);
    IChildStage MakeChildSpan<T>(int length, string module, int baseOffset, params int[] offsets);
    IChildStage MakeChildSpan<T>(int length, Module module, int baseOffset, params int[] offsets);
    IChildStage MakeChildSpan<T>(int length, nint baseAddress, params int[] offsets);

    IChildStage MakeChildString(int baseOffset, params int[] offsets);
    IChildStage MakeChildString(int length, int baseOffset, params int[] offsets);
    IChildStage MakeChildString(ReadStringType stringType, int baseOffset, params int[] offsets);
    IChildStage MakeChildString(int length, ReadStringType stringType, int baseOffset, params int[] offsets);

    IChildStage MakeChildString(string module, int baseOffset, params int[] offsets);
    IChildStage MakeChildString(int length, string module, int baseOffset, params int[] offsets);
    IChildStage MakeChildString(ReadStringType stringType, string module, int baseOffset, params int[] offsets);
    IChildStage MakeChildString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets);

    IChildStage MakeChildString(Module module, int baseOffset, params int[] offsets);
    IChildStage MakeChildString(int length, Module module, int baseOffset, params int[] offsets);
    IChildStage MakeChildString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets);
    IChildStage MakeChildString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets);

    IChildStage MakeChildString(nint baseAddress, params int[] offsets);
    IChildStage MakeChildString(int length, nint baseAddress, params int[] offsets);
    IChildStage MakeChildString(ReadStringType stringType, nint baseAddress, params int[] offsets);
    IChildStage MakeChildString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets);
}
