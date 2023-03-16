using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal static partial class ResolveBaseAddress
{
    public readonly record struct FromMainModule(int BaseOffset) : IBaseAddressResolver
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
}
