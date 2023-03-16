using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal static class MakePointer<T>
    where T : unmanaged
{
    public readonly record struct Command
        : IMakePointerCommand
    {
        private readonly IBaseAddressResolver _resolver;
        private readonly int[] _offsets;

        public Command(nint baseAddress, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromAbsolute(baseAddress);
            _offsets = offsets;
        }

        public Command(int baseOffset, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromMainModule(baseOffset);
            _offsets = offsets;
        }

        public Command(string moduleName, int baseOffset, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromModuleName(moduleName, baseOffset);
            _offsets = offsets;
        }

        public Command(Module module, int baseOffset, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromModule(module, baseOffset);
            _offsets = offsets;
        }

        public bool TryExecute(IMemoryManager manager, out IPointer pointer)
        {
            if (!_resolver.TryResolve(manager, out nint baseAddress))
            {
                pointer = default;
                return false;
            }

            pointer = new Pointer<T>(manager, baseAddress, _offsets);
            return true;
        }
    }
}
