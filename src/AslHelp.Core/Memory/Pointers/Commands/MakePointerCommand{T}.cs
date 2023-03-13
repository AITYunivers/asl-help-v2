using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Commands;

internal sealed class MakePointerCommand<T>
    : MakePointerCommandBase
    where T : unmanaged
{
    public MakePointerCommand(int baseOffset, int[] offsets)
        : base(baseOffset, offsets) { }

    public MakePointerCommand(string moduleName, int baseOffset, params int[] offsets)
        : base(moduleName, baseOffset, offsets) { }

    public MakePointerCommand(Module module, int baseOffset, params int[] offsets)
        : base(module, baseOffset, offsets) { }

    public MakePointerCommand(nint baseAddress, int[] offsets)
        : base(baseAddress, offsets) { }

    public override bool TryExecute(IMemoryManager manager, out IPointer pointer)
    {
        if (!TryGetBaseAddress(manager, out nint baseAddress))
        {
            pointer = default;
            return false;
        }

        pointer = new Pointer<T>(manager, baseAddress, _offsets);
        return true;
    }
}
