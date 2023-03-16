namespace AslHelp.Core.Memory.Pointers.Construction;

public class LazyPointerFactory
    : ILazyPointerFactory
{
    private readonly List<IPointer<nint>> _parents = new();

    public ILazyPointerFactory MakeParent(nint baseAddress, params int[] offsets)
    {
        if (_parents.Count > 0)
        {
            IPointer<nint> parent = _parents[^1];

        }
    }
}
