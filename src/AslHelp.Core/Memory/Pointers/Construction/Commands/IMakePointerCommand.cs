using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal interface IMakePointerCommand
{
    IMakePointerCommand Parent { get; set; }
    IPointer Result { get; }

    bool TryExecute(IMemoryManager manager, out IPointer pointer);
}

internal abstract class MakePointerCommandBase
    : IMakePointerCommand
{
    protected readonly IBaseAddressResolver _resolver;
    protected readonly int[] _offsets;

    protected readonly IMakePointerCommand _parent;

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

    public MakePointerCommandBase(IMakePointerCommand parent, int[] offsets)
    {
        _parent = parent;
        _offsets = offsets;
    }

    public IPointer Result { get; protected set; }

    public abstract bool TryExecute(IMemoryManager manager, out IPointer pointer);
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

    public override bool TryExecute(IMemoryManager manager, out IPointer pointer)
    {
        if (!_resolver.TryResolve(manager, out nint baseAddress))
        {
            pointer = default;
            return false;
        }
    }
}
