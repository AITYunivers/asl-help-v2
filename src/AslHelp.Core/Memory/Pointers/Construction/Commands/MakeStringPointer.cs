using AslHelp.Core.Memory.IO;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal sealed class MakeStringPointer
    : MakePointerCommandBase<StringPointer>
{
    private readonly bool _sized;

    private readonly int _length;
    private readonly ReadStringType _stringType;

    public MakeStringPointer(int length, ReadStringType stringType, nint baseAddress, int[] offsets)
        : base(baseAddress, offsets)
    {
        _sized = false;

        _length = length;
        _stringType = stringType;
    }

    public MakeStringPointer(int length, ReadStringType stringType, int baseOffset, int[] offsets)
        : base(baseOffset, offsets)
    {
        _sized = false;

        _length = length;
        _stringType = stringType;
    }

    public MakeStringPointer(int length, ReadStringType stringType, string moduleName, int baseOffset, int[] offsets)
        : base(moduleName, baseOffset, offsets)
    {
        _sized = false;

        _length = length;
        _stringType = stringType;
    }

    public MakeStringPointer(int length, ReadStringType stringType, Module module, int baseOffset, int[] offsets)
        : base(module, baseOffset, offsets)
    {
        _sized = false;

        _length = length;
        _stringType = stringType;
    }

    public MakeStringPointer(int length, ReadStringType stringType, IMakePointerCommand parent, int firstOffset, int[] offsets)
        : base(parent, firstOffset, offsets)
    {
        _sized = false;

        _length = length;
        _stringType = stringType;
    }

    public MakeStringPointer(ReadStringType stringType, nint baseAddress, int[] offsets)
        : base(baseAddress, offsets)
    {
        _sized = true;

        _stringType = stringType;
    }

    public MakeStringPointer(ReadStringType stringType, int baseOffset, int[] offsets)
        : base(baseOffset, offsets)
    {
        _sized = true;

        _stringType = stringType;
    }

    public MakeStringPointer(ReadStringType stringType, string moduleName, int baseOffset, int[] offsets)
        : base(moduleName, baseOffset, offsets)
    {
        _sized = true;

        _stringType = stringType;
    }

    public MakeStringPointer(ReadStringType stringType, Module module, int baseOffset, int[] offsets)
        : base(module, baseOffset, offsets)
    {
        _sized = true;

        _stringType = stringType;
    }

    public MakeStringPointer(ReadStringType stringType, IMakePointerCommand parent, int firstOffset, int[] offsets)
        : base(parent, firstOffset, offsets)
    {
        _sized = true;

        _stringType = stringType;
    }

    protected override StringPointer Make(IMemoryManager manager, nint baseAddress, int[] offsets)
    {
        return _sized
            ? new(manager, _stringType, baseAddress, offsets)
            : new(manager, _length, _stringType, baseAddress, offsets);
    }

    protected override StringPointer MakeFromParent(IMemoryManager manager, IPointer<nint> parent, int firstOffset, int[] offsets)
    {
        return _sized
            ? new(manager, _stringType, parent, firstOffset, offsets)
            : new(manager, _length, _stringType, parent, firstOffset, offsets);
    }
}
