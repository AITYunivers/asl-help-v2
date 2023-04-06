using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal static partial class ResolveBaseAddress
{
    private readonly struct MainModuleResolver
        : IBaseAddressResolver
    {
        private readonly int _baseOffset;

        public MainModuleResolver(int baseOffset)
        {
            _baseOffset = baseOffset;
        }

        public bool TryResolve(IMemoryManager manager, out nint baseAddress)
        {
            if (manager.MainModule is { Base: > 0 } module)
            {
                baseAddress = module.Base + _baseOffset;
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
