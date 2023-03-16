using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal interface IBaseAddressResolver
{
    bool TryResolve(IMemoryManager manager, out nint baseAddress);
}
