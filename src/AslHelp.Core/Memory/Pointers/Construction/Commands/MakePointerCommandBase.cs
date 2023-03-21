using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal abstract class MakePointerCommandBase<TPointer>
    : IMakePointerCommand
    where TPointer : IPointer
{
    private readonly IBaseAddressResolver _resolver;
    private readonly IMakePointerCommand _parent;

    private readonly int _firstOffset;
    private readonly int[] _offsets;

    private IPointer _result;

    private string _name;
    private bool _logChange;
    private bool _updateOnFail;

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

    protected abstract TPointer Make(IMemoryManager manager, nint baseAddress, int[] offsets);
    protected abstract TPointer MakeFromParent(IMemoryManager manager, IPointer<nint> parent, int firstOffset, int[] offsets);

    public void Name(string name)
    {
        _name = name;
    }

    public void LogChange()
    {
        _logChange = true;
    }

    public void UpdateOnFail()
    {
        _updateOnFail = true;
    }

    public bool TryExecute(IMemoryManager manager)
    {
        if (Result is not null)
        {
            return true;
        }

        if (_parent is not null)
        {
            if (_parent.TryExecute(manager) && _parent.Result is IPointer<nint> parent)
            {
                Result = MakeFromParent(manager, parent, _firstOffset, _offsets);
                return true;
            }
        }
        else
        {
            if (_resolver.TryResolve(manager, out nint baseAddress))
            {
                Result = Make(manager, baseAddress, _offsets);
                return true;
            }
        }

        return false;
    }
}
