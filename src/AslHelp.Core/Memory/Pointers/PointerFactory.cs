using AslHelp.Core.Exceptions;
using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers;

public class PointerFactory
{
    private readonly IMemoryManager _manager;

    public PointerFactory(IMemoryManager manager)
    {
        ThrowHelper.ThrowIfNull(manager);
        _manager = manager;
    }

    public Pointer<T> Make<T>(int baseOffset, params int[] offsets) where T : unmanaged
    {
        return null;
    }
}
