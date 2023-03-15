namespace AslHelp.Core.Memory.Pointers.Construction;

public interface ILazyPointerFactory
{
    ILazyPointerFactory MakeParent(nint baseAddress, params int[] offsets);
}
