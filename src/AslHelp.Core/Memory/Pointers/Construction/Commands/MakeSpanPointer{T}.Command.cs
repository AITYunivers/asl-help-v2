using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal static class MakeSpanPointer<T>
    where T : unmanaged
{
    public readonly record struct Command
        : IMakePointerCommand
    {
        private readonly IBaseAddressResolver _resolver;
        private readonly int _length;
        private readonly int[] _offsets;

        public Command(int length, nint baseAddress, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromAbsolute(baseAddress);
            _length = length;
            _offsets = offsets;
        }

        public Command(int length, int baseOffset, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromMainModule(baseOffset);
            _length = length;
            _offsets = offsets;
        }

        public Command(int length, string moduleName, int baseOffset, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromModuleName(moduleName, baseOffset);
            _length = length;
            _offsets = offsets;
        }

        public Command(int length, Module module, int baseOffset, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromModule(module, baseOffset);
            _length = length;
            _offsets = offsets;
        }

        public bool TryExecute(IMemoryManager manager, out IPointer pointer)
        {
            if (!_resolver.TryResolve(manager, out nint baseAddress))
            {
                pointer = default;
                return false;
            }

            pointer = new SpanPointer<T>(manager, _length, baseAddress, _offsets);
            return true;
        }
    }
}
