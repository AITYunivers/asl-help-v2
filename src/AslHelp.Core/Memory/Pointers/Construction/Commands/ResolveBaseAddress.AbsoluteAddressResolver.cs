using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal static partial class ResolveBaseAddress
{
    private readonly struct AbsoluteAddressResolver
        : IBaseAddressResolver
    {
        private readonly nint _baseAddress;

        public AbsoluteAddressResolver(nint baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public bool TryResolve(IMemoryManager _, out nint baseAddress)
        {
            baseAddress = _baseAddress;
            return true;
        }
    }
}
