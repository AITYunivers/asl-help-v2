using System.Collections.Generic;
using System.Linq;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public IEnumerable<nuint> ScanAllRel(int offset, params string[] pattern)
    {
        return ScanAll(offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(int offset, params byte[] pattern)
    {
        return ScanAll(offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(string moduleName, int offset, params string[] pattern)
    {
        return ScanAll(moduleName, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(string moduleName, int offset, params byte[] pattern)
    {
        return ScanAll(moduleName, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(Module module, int offset, params string[] pattern)
    {
        return ScanAll(module, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(Module module, int offset, params byte[] pattern)
    {
        return ScanAll(module, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(nuint startAddress, nuint endAddress, int offset, params string[] pattern)
    {
        return ScanAll(startAddress, endAddress, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(nuint startAddress, nuint endAddress, int offset, params byte[] pattern)
    {
        return ScanAll(startAddress, endAddress, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(nuint startAddress, uint size, int offset, params string[] pattern)
    {
        return ScanAll(startAddress, size, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(nuint startAddress, uint size, int offset, params byte[] pattern)
    {
        return ScanAll(startAddress, size, offset, pattern).Select(FromAssemblyAddress);
    }
}
