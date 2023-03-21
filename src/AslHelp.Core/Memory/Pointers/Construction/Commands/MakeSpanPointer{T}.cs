using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal sealed class MakeSpanPointer<T>
    : MakePointerCommandBase<SpanPointer<T>>
    where T : unmanaged
{
    private readonly int _length;

    public MakeSpanPointer(int length, nint baseAddress, int[] offsets)
        : base(baseAddress, offsets)
    {
        _length = length;
    }

    public MakeSpanPointer(int length, int baseOffset, int[] offsets)
        : base(baseOffset, offsets)
    {
        _length = length;
    }

    public MakeSpanPointer(int length, string moduleName, int baseOffset, int[] offsets)
        : base(moduleName, baseOffset, offsets)
    {
        _length = length;
    }

    public MakeSpanPointer(int length, Module module, int baseOffset, int[] offsets)
        : base(module, baseOffset, offsets)
    {
        _length = length;
    }

    public MakeSpanPointer(int length, IMakePointerCommand parent, int firstOffset, int[] offsets)
        : base(parent, firstOffset, offsets)
    {
        _length = length;
    }

    protected override SpanPointer<T> Make(IMemoryManager manager, nint baseAddress, int[] offsets)
    {
        return new(manager, _length, baseAddress, offsets);
    }

    protected override SpanPointer<T> MakeFromParent(IMemoryManager manager, IPointer<nint> parent, int firstOffset, int[] offsets)
    {
        return new(manager, _length, parent, firstOffset, offsets);
    }
}
