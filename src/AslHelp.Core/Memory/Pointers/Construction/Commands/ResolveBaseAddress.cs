using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Construction.Commands;

internal interface IBaseAddressResolver
{
    bool TryResolve(IMemoryManager manager, out nint baseAddress);
}

internal static partial class ResolveBaseAddress
{
    public static IBaseAddressResolver FromAbsolute(nint baseAddress)
    {
        return new AbsoluteAddressResolver(baseAddress);
    }

    public static IBaseAddressResolver FromMainModule(int baseOffset)
    {
        return new MainModuleResolver(baseOffset);
    }

    public static IBaseAddressResolver FromModuleName(string moduleName, int baseOffset)
    {
        return new ModuleNameResolver(moduleName, baseOffset);
    }

    public static IBaseAddressResolver FromModule(Module module, int baseOffset)
    {
        return new ModuleResolver(module, baseOffset);
    }
}
