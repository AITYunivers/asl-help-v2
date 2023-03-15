using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction;

internal interface IMakePointerCommand
{
    bool TryExecute(IMemoryManager manager, out IPointer pointer);
}

internal interface IBaseAddressResolver
{
    bool TryResolve(IMemoryManager manager, out nint baseAddress);
}

internal static class ResolveBaseAddress
{
    public record FromAbsolute(nint BaseAddress) : IBaseAddressResolver
    {
        public bool TryResolve(IMemoryManager _, out nint baseAddress)
        {
            baseAddress = BaseAddress;
            return true;
        }
    }

    public record FromMainModule(int BaseOffset) : IBaseAddressResolver
    {
        public bool TryResolve(IMemoryManager manager, out nint baseAddress)
        {
            if (manager.MainModule is { Base: > 0 } module)
            {
                baseAddress = module.Base + BaseOffset;
                return true;
            }
            else
            {
                baseAddress = default;
                return false;
            }
        }
    }

    public record FromModuleName(string ModuleName, int BaseOffset) : IBaseAddressResolver
    {
        public bool TryResolve(IMemoryManager manager, out nint baseAddress)
        {
            if (manager.Modules[ModuleName] is { Base: > 0 } module)
            {
                baseAddress = module.Base + BaseOffset;
                return true;
            }
            else
            {
                baseAddress = default;
                return false;
            }
        }
    }

    public record FromModule(Module Module, int BaseOffset) : IBaseAddressResolver
    {
        public bool TryResolve(IMemoryManager manager, out nint baseAddress)
        {
            if (Module.Base > 0)
            {
                baseAddress = Module.Base + BaseOffset;
                return true;
            }
            else
            {
                baseAddress = default;
                return false;
            }
        }
    }
}

internal abstract class MakePointerCommandBase
    : IMakePointerCommand
{
    private readonly IBaseAddressResolver _resolver;

    public MakePointerCommandBase(nint baseAddress)
    {
        _resolver = new ResolveBaseAddress.FromAbsolute(baseAddress);
    }

    public abstract bool TryExecute(IMemoryManager manager, out IPointer pointer);
}

internal static class MakePointer<T>
    where T : unmanaged
{
    public class Command
    {

    }
}

public class LazyPointerFactory
    : ILazyPointerFactory
{
    private readonly List<IPointer<nint>> _parents = new();

    public ILazyPointerFactory MakeParent(nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }
}
