using System.Collections.Generic;
using System.Linq;

namespace AslHelp.Core.Memory.Ipc;

public abstract partial class MemoryManagerBase
{
    public IEnumerable<nint> ScanAllRel(Signature signature, int alignment = 1)
    {
        return ScanAll(signature, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, int size, int alignment = 1)
    {
        return ScanAll(signature, size, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, string moduleName, int alignment = 1)
    {
        return ScanAll(signature, moduleName, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, string moduleName, int size, int alignment = 1)
    {
        return ScanAll(signature, moduleName, size, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, Module module, int alignment = 1)
    {
        return ScanAll(signature, module, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, Module module, int size, int alignment = 1)
    {
        return ScanAll(signature, module, size, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, nint startAddress, nint endAddress, int alignment = 1)
    {
        return ScanAll(signature, startAddress, endAddress, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, nint startAddress, int size, int alignment = 1)
    {
        return ScanAll(signature, startAddress, size, alignment).Select(FromAssemblyAddress);
    }
}
