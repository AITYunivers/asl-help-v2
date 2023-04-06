namespace AslHelp.Core.Memory.Pointers.Construction;

public interface ILazyPointerFactory
{
    ILazyPointerFactory MakeParent(nint baseAddress, params int[] offsets);
    ILazyPointerFactory MakeParent(int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeParent(string moduleName, int baseOffset, params int[] offsets);
    ILazyPointerFactory MakeParent(Module module, int baseOffset, params int[] offsets);
}
