using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal sealed class MakePointer<T>
    : MakePointerCommandBase<Pointer<T>>
    where T : unmanaged
{
    public MakePointer(nint baseAddress, int[] offsets)
        : base(baseAddress, offsets) { }

    public MakePointer(int baseOffset, int[] offsets)
        : base(baseOffset, offsets) { }

    public MakePointer(string moduleName, int baseOffset, int[] offsets)
        : base(moduleName, baseOffset, offsets) { }

    public MakePointer(Module module, int baseOffset, int[] offsets)
        : base(module, baseOffset, offsets) { }

    public MakePointer(IMakePointerCommand parent, int firstOffset, int[] offsets)
        : base(parent, firstOffset, offsets) { }

    protected override Pointer<T> Make(IMemoryManager manager, nint baseAddress, int[] offsets)
    {
        return new(manager, baseAddress, offsets);
    }

    protected override Pointer<T> MakeFromParent(IMemoryManager manager, IPointer<nint> parent, int firstOffset, int[] offsets)
    {
        return new(manager, parent, firstOffset, offsets);
    }
}
