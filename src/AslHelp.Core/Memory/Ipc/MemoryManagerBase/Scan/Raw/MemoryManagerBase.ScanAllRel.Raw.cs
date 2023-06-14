using System.Collections.Generic;
using System.Linq;

namespace AslHelp.Core.Memory.Ipc;

public abstract partial class MemoryManagerBase
{
    public IEnumerable<nint> ScanAllRel(int offset, params string[] pattern)
    {
        return ScanAll(offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(int offset, params byte[] pattern)
    {
        return ScanAll(offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(string moduleName, int offset, params string[] pattern)
    {
        return ScanAll(moduleName, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(string moduleName, int offset, params byte[] pattern)
    {
        return ScanAll(moduleName, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Module module, int offset, params string[] pattern)
    {
        return ScanAll(module, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Module module, int offset, params byte[] pattern)
    {
        return ScanAll(module, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(nint startAddress, nint endAddress, int offset, params string[] pattern)
    {
        return ScanAll(startAddress, endAddress, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(nint startAddress, nint endAddress, int offset, params byte[] pattern)
    {
        return ScanAll(startAddress, endAddress, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(nint startAddress, int size, int offset, params string[] pattern)
    {
        return ScanAll(startAddress, size, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(nint startAddress, int size, int offset, params byte[] pattern)
    {
        return ScanAll(startAddress, size, offset, pattern).Select(FromAssemblyAddress);
    }
}
