using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Construction;

public interface ILazyPointerFactory
{
    ILazyPointerFactory Make<T>(nint baseAddress, params int[] offsets) where T : unmanaged;
    ILazyPointerFactory Make<T>(int baseOffset, params int[] offsets) where T : unmanaged;
    ILazyPointerFactory Make<T>(string moduleName, int baseOffset, params int[] offsets) where T : unmanaged;

    ILazyPointerFactory MakeSpan<T>(int length, nint baseAddress, params int[] offsets) where T : unmanaged;
    ILazyPointerFactory MakeSpan<T>(int length, int baseOffset, params int[] offsets) where T : unmanaged;
    ILazyPointerFactory MakeSpan<T>(int length, string moduleName, int baseOffset, params int[] offsets) where T : unmanaged;

    ILazyPointerFactory MakeString(nint baseAddress, params int[] offsets);
    ILazyPointerFactory MakeString(int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeString(string moduleName, int baseOffset, params int[] offsets);

    ILazyPointerFactory MakeString(int length, nint baseAddress, params int[] offsets);
    ILazyPointerFactory MakeString(int length, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeString(int length, string moduleName, int baseOffset, params int[] offsets);

    ILazyPointerFactory MakeString(ReadStringType stringType, nint baseAddress, params int[] offsets);
    ILazyPointerFactory MakeString(ReadStringType stringType, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets);

    ILazyPointerFactory MakeString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets);
    ILazyPointerFactory MakeString(int length, ReadStringType stringType, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeString(int length, ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets);

    ILazyPointerFactory MakeSizedString(nint baseAddress, params int[] offsets);
    ILazyPointerFactory MakeSizedString(int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeSizedString(string moduleName, int baseOffset, params int[] offsets);

    ILazyPointerFactory MakeSizedString(ReadStringType stringType, nint baseAddress, params int[] offsets);
    ILazyPointerFactory MakeSizedString(ReadStringType stringType, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeSizedString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets);
}

public interface ILazyParentPointerFactory
    : ILazyPointerFactory
{
    ILazyChildPointerFactory MakeParent(nint baseAddress, params int[] offsets);
    ILazyChildPointerFactory MakeParent(int baseOffset, params int[] offsets);
    ILazyChildPointerFactory MakeParent(string moduleName, int baseOffset, params int[] offsets);
    ILazyChildPointerFactory MakeParent(Module module, int baseOffset, params int[] offsets);
}

public interface ILazyChildPointerFactory
{
    ILazyPointerFactory Top { get; }

    ILazyChildPointerFactory Make<T>(int nextOffset, params int[] offsets) where T : unmanaged;

    ILazyChildPointerFactory MakeSpan<T>(int nextOffset, params int[] offsets) where T : unmanaged;

    ILazyChildPointerFactory MakeString(int nextOffset, params int[] offsets);
    ILazyChildPointerFactory MakeString(int length, int nextOffset, params int[] offsets);
    ILazyChildPointerFactory MakeString(ReadStringType stringType, int nextOffset, params int[] offsets);
    ILazyChildPointerFactory MakeString(int length, ReadStringType stringType, int nextOffset, params int[] offsets);

    ILazyChildPointerFactory MakeSizedString(int nextOffset, params int[] offsets);
    ILazyChildPointerFactory MakeSizedString(ReadStringType stringType, int nextOffset, params int[] offsets);
}

public interface ILazyChildParentPointerFactory
    : ILazyChildPointerFactory
{
    ILazyChildPointerFactory MakeParent(int nextOffset, params int[] offsets);

    public interface IFirst
    {
        ILazyParentPointerFactory Previous { get; }
    }

    public interface INested
    {
        ILazyChildParentPointerFactory Previous { get; }
    }
}

public interface ILazyHierarchicalPointerFactory
    : ILazyPointerFactory,
    ILazyChildParentPointerFactory.IFirst,
    ILazyChildParentPointerFactory.INested
{ }
