using AslHelp.Core.Exceptions;
using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal static partial class ResolveBaseAddress
{
    private readonly struct ModuleResolver
        : IBaseAddressResolver
    {
        private readonly Module _module;
        private readonly int _baseOffset;

        public ModuleResolver(Module module, int baseOffset)
        {
            ThrowHelper.ThrowIfNull(module);
            ThrowHelper.ThrowIfLessThan(baseOffset, 0);

            _module = module;
            _baseOffset = baseOffset;
        }

        public bool TryResolve(IMemoryManager manager, out nint baseAddress)
        {
            if (_module.Base > 0)
            {
                baseAddress = _module.Base + _baseOffset;
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
