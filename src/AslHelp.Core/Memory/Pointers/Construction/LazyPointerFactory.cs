using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Construction;

public class LazyPointerFactory
    : ILazyPointerFactory,
    IParentStage,
    IChildStage,
    IChildOrNext
{
    ILazyPointerFactory ILazyPointerFactory.Make<T>(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.Make<T>(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.Make<T>(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.Make<T>(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChild<T>(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChild<T>(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChild<T>(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChild<T>(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChild<T>(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChild<T>(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChild<T>(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChild<T>(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildSpan<T>(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildSpan<T>(int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildSpan<T>(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildSpan<T>(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildSpan<T>(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildSpan<T>(int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildSpan<T>(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildSpan<T>(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildStage IParentStage.MakeChildString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IChildOrNext IChildStage.MakeChildString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IParentStage ILazyPointerFactory.MakeParent(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IParentStage ILazyPointerFactory.MakeParent(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IParentStage ILazyPointerFactory.MakeParent(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    IParentStage ILazyPointerFactory.MakeParent(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeSpan<T>(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeSpan<T>(int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeSpan<T>(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeSpan<T>(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyPointerFactory ILazyPointerFactory.MakeString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }
}
