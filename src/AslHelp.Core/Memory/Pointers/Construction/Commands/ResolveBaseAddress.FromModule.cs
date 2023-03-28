using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal static partial class ResolveBaseAddress
{
    public readonly record struct FromModule(Module Module, int BaseOffset) : IBaseAddressResolver
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
