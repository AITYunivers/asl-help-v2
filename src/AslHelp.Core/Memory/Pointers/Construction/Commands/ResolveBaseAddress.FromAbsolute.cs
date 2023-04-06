using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal static partial class ResolveBaseAddress
{
    public readonly record struct FromAbsolute(nint BaseAddress) : IBaseAddressResolver
    {
        public bool TryResolve(IMemoryManager _, out nint baseAddress)
        {
            baseAddress = BaseAddress;
            return true;
        }
    }
}
