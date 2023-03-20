using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal interface IMakePointerCommand
{
    IPointer Result { get; }

    bool TryExecute(IMemoryManager manager);
}

internal sealed class PointerCommand
{
    public static PointerCommand Make<T>(nint baseAddress, int[] offsets)
    {

    }
}

internal abstract class MakePointerCommandBase
    : IMakePointerCommand
{
    private readonly IBaseAddressResolver _resolver;
    private readonly IMakePointerCommand _parent;

    protected readonly int _firstOffset;
    protected readonly int[] _offsets;

    public MakePointerCommandBase(nint baseAddress, int[] offsets)
    {
        _resolver = new ResolveBaseAddress.FromAbsolute(baseAddress);
        _offsets = offsets;
    }

    public MakePointerCommandBase(int baseOffset, int[] offsets)
    {
        _resolver = new ResolveBaseAddress.FromMainModule(baseOffset);
        _offsets = offsets;
    }

    public MakePointerCommandBase(string moduleName, int baseOffset, int[] offsets)
    {
        _resolver = new ResolveBaseAddress.FromModuleName(moduleName, baseOffset);
        _offsets = offsets;
    }

    public MakePointerCommandBase(Module module, int baseOffset, int[] offsets)
    {
        _resolver = new ResolveBaseAddress.FromModule(module, baseOffset);
        _offsets = offsets;
    }

    public MakePointerCommandBase(IMakePointerCommand parent, int firstOffset, int[] offsets)
    {
        _parent = parent;
        _firstOffset = firstOffset;
        _offsets = offsets;
    }

    public IPointer Result { get; protected set; }

    protected abstract IPointer Make(IMemoryManager manager, nint baseAddress);
    protected abstract IPointer MakeFromParent(IMemoryManager manager);

    public bool TryExecute(IMemoryManager manager, out IPointer pointer)
    {
        if (_parent is not null)
        {
            if (_parent.Result is IPointer<nint> parent)
            {
                pointer = MakeFromParent(manager, parent);
                return true;
            }
        }
        else
        {
            if (_resolver.TryResolve(manager, out nint baseAddress))
            {
                Result = pointer = Make(manager, baseAddress);
                return true;
            }
        }

        pointer = default;
        return false;
    }
}

internal sealed class MakePointerCommand<T>
    : MakePointerCommandBase
    where T : unmanaged
{
    public MakePointerCommand(nint baseAddress, int[] offsets)
        : base(baseAddress, offsets) { }

    public MakePointerCommand(int baseOffset, int[] offsets)
        : base(baseOffset, offsets) { }

    public MakePointerCommand(string moduleName, int baseOffset, int[] offsets)
        : base(moduleName, baseOffset, offsets) { }

    public MakePointerCommand(Module module, int baseOffset, int[] offsets)
        : base(module, baseOffset, offsets) { }

    public MakePointerCommand(IMakePointerCommand parent, int firstOffset, int[] offsets)
        : base(parent, firstOffset, offsets) { }

    protected override IPointer Make(IMemoryManager manager, nint baseAddress)
    {
        return new Pointer<T>(manager, baseAddress, _offsets);
    }

    protected override IPointer MakeFromParent(IMemoryManager manager, IPointer<nint> parent)
    {
        return new Pointer<T>(manager, parent, _firstOffset, _offsets);
    }
}
