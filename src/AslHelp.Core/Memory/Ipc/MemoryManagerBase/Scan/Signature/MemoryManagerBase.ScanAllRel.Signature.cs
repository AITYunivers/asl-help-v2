using System.Collections.Generic;
using System.Linq;

using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public IEnumerable<nuint> ScanAllRel(Signature signature, uint alignment = 1)
    {
        return ScanAll(signature, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(uint size, Signature signature, uint alignment = 1)
    {
        return ScanAll(size, signature, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(string moduleName, Signature signature, uint alignment = 1)
    {
        return ScanAll(moduleName, signature, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(string moduleName, uint size, Signature signature, uint alignment = 1)
    {
        return ScanAll(moduleName, size, signature, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(Module module, Signature signature, uint alignment = 1)
    {
        return ScanAll(module, signature, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(Module module, uint size, Signature signature, uint alignment = 1)
    {
        return ScanAll(module, size, signature, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(nuint startAddress, nuint endAddress, Signature signature, uint alignment = 1)
    {
        return ScanAll(startAddress, endAddress, signature, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(nuint startAddress, uint size, Signature signature, uint alignment = 1)
    {
        return ScanAll(startAddress, size, signature, alignment).Select(FromAssemblyAddress);
    }
}
