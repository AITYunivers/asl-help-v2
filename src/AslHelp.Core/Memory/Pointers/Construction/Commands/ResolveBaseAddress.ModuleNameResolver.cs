using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal static partial class ResolveBaseAddress
{
    private readonly struct ModuleNameResolver
        : IBaseAddressResolver
    {
        private readonly string _moduleName;
        private readonly int _baseOffset;

        public ModuleNameResolver(string moduleName, int baseOffset)
        {
            _moduleName = moduleName;
            _baseOffset = baseOffset;
        }

        public bool TryResolve(IMemoryManager manager, out nint baseAddress)
        {
            string name = _moduleName;
            if (manager.Modules.FirstOrDefault(m => m.Name == name) is { Base: > 0 } module)
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
