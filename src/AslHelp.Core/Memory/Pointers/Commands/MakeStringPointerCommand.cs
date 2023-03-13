using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Commands;

internal sealed class MakeStringPointerCommand<T>
    : MakePointerCommandBase
    where T : unmanaged
{
    public MakeStringPointerCommand(int baseOffset, int[] offsets)
        : base(baseOffset, offsets) { }

    public MakeStringPointerCommand(string moduleName, int baseOffset, params int[] offsets)
        : base(moduleName, baseOffset, offsets) { }

    public MakeStringPointerCommand(Module module, int baseOffset, params int[] offsets)
        : base(module, baseOffset, offsets) { }

    public MakeStringPointerCommand(nint baseAddress, int[] offsets)
        : base(baseAddress, offsets) { }

    public override bool TryExecute(IMemoryManager manager, out IPointer pointer)
    {
        if (!TryGetBaseAddress(manager, out nint baseAddress))
        {
            pointer = default;
            return false;
        }

        pointer = new StringPointer(manager, baseAddress, _offsets);
        return true;
    }
}
