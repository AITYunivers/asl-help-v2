using AslHelp.Core.Memory.IO;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal static class MakeStringPointer
{
    public readonly record struct Command
        : IMakePointerCommand
    {
        private readonly IBaseAddressResolver _resolver;
        private readonly int? _length;
        private readonly ReadStringType _stringType;
        private readonly int[] _offsets;

        public Command(int length, ReadStringType stringType, nint baseAddress, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromAbsolute(baseAddress);
            _length = length;
            _stringType = stringType;
            _offsets = offsets;
        }

        public Command(int length, ReadStringType stringType, int baseOffset, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromMainModule(baseOffset);
            _length = length;
            _stringType = stringType;
            _offsets = offsets;
        }

        public Command(int length, ReadStringType stringType, string moduleName, int baseOffset, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromModuleName(moduleName, baseOffset);
            _length = length;
            _stringType = stringType;
            _offsets = offsets;
        }

        public Command(int length, ReadStringType stringType, Module module, int baseOffset, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromModule(module, baseOffset);
            _length = length;
            _stringType = stringType;
            _offsets = offsets;
        }

        public Command(ReadStringType stringType, nint baseAddress, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromAbsolute(baseAddress);
            _stringType = stringType;
            _offsets = offsets;
        }

        public Command(ReadStringType stringType, int baseOffset, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromMainModule(baseOffset);
            _stringType = stringType;
            _offsets = offsets;
        }

        public Command(ReadStringType stringType, string moduleName, int baseOffset, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromModuleName(moduleName, baseOffset);
            _stringType = stringType;
            _offsets = offsets;
        }

        public Command(ReadStringType stringType, Module module, int baseOffset, int[] offsets)
        {
            _resolver = new ResolveBaseAddress.FromModule(module, baseOffset);
            _stringType = stringType;
            _offsets = offsets;
        }

        public bool TryExecute(IMemoryManager manager, out IPointer pointer)
        {
            if (!_resolver.TryResolve(manager, out nint baseAddress))
            {
                pointer = default;
                return false;
            }

            pointer =
                _length is int length
                ? new StringPointer(manager, length, _stringType, baseAddress, _offsets)
                : new StringPointer(manager, _stringType, baseAddress, _offsets);
            return true;
        }
    }
}
