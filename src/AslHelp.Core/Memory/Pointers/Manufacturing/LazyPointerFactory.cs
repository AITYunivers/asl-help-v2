using AslHelp.Core.Memory.IO;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Manufacturing;

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
}

public interface IParentState
{
    IParentState MakeParent();
    IChildState MakeChild();
}

public interface IChildState
{
    IChildOrNext MakeChild();
}

public interface IChildOrNext
    : ILazyPointerFactory,
    IChildState
{ }

public class LazyPointerFactory
{
    private readonly List<Func<IMemoryManager, IPointer>> _makers = new();

    public LazyPointerFactory Make<T>(string module, int baseOffset, params int[] offsets)
        where T : unmanaged
    {
        _makers.Add(manager => new Pointer<T>(manager, manager.Modules[module].Base + baseOffset, offsets));

        return this;
    }
}
