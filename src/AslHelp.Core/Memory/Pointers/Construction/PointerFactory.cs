using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Construction;

public class PointerFactory
    : ILazyPointerFactory
{
    private readonly Stack<ILazyChildPointerFactory> _children = new();

    protected PointerFactory()
    {
        Top = this;
        Previous = this;
    }

    public ITopLevelPointerFactory Top { get; private set; }
    public ITopLevelPointerFactory Previous { get; private set; }

    ILazyChildPointerFactory ILazyChildPointerFactory.INested.Previous => _children.Pop();

    ITopLevelPointerFactory ITopLevelPointerFactory.Make<T>(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.Make<T>(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.Make<T>(string moduleName, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.Make<T>(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyChildPointerFactory ILazyChildPointerFactory.Make<T>(int nextOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyChildPointerFactory ITopLevelPointerFactory.MakeParent(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyChildPointerFactory ITopLevelPointerFactory.MakeParent(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyChildPointerFactory ITopLevelPointerFactory.MakeParent(string moduleName, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyChildPointerFactory ITopLevelPointerFactory.MakeParent(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyChildPointerFactory ILazyChildPointerFactory.MakeParent(int nextOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeSizedString(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeSizedString(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeSizedString(string moduleName, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeSizedString(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeSizedString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeSizedString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeSizedString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeSizedString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyChildPointerFactory ILazyChildPointerFactory.MakeSizedString(int nextOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyChildPointerFactory ILazyChildPointerFactory.MakeSizedString(ReadStringType stringType, int nextOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeSpan<T>(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeSpan<T>(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeSpan<T>(int length, string moduleName, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeSpan<T>(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyChildPointerFactory ILazyChildPointerFactory.MakeSpan<T>(int nextOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(string moduleName, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(int length, string moduleName, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(int length, ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ITopLevelPointerFactory ITopLevelPointerFactory.MakeString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyChildPointerFactory ILazyChildPointerFactory.MakeString(int nextOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyChildPointerFactory ILazyChildPointerFactory.MakeString(int length, int nextOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyChildPointerFactory ILazyChildPointerFactory.MakeString(ReadStringType stringType, int nextOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    ILazyChildPointerFactory ILazyChildPointerFactory.MakeString(int length, ReadStringType stringType, int nextOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }
}
