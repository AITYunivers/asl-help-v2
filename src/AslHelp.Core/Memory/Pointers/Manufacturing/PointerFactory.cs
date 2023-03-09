using AslHelp.Core.Exceptions;
using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Manufacturing;

public class PointerFactory
{
    private readonly IMemoryManager _manager;

    public PointerFactory(IMemoryManager manager)
    {
        ThrowHelper.ThrowIfNull(manager);

        _manager = manager;
    }

    public Pointer<T> Make<T>(nint baseOffset, params int[] offsets) where T : unmanaged
    {
        return new(_manager, baseOffset, offsets);
    }

    public Pointer<T> Make<T>(IPointer<nint> parent, int firstOffset, params int[] offsets) where T : unmanaged
    {
        return new(_manager, parent, firstOffset, offsets);
    }
}
