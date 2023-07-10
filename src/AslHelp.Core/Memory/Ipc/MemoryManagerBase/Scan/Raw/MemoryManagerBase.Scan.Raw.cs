using System.Linq;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint Scan(uint offset, params string[] pattern)
    {
        return ScanAll(offset, pattern).FirstOrDefault();
    }

    public nuint Scan(uint offset, params byte[] pattern)
    {
        return ScanAll(offset, pattern).FirstOrDefault();
    }

    public nuint Scan(string moduleName, uint offset, params string[] pattern)
    {
        return ScanAll(moduleName, offset, pattern).FirstOrDefault();
    }

    public nuint Scan(string moduleName, uint offset, params byte[] pattern)
    {
        return ScanAll(moduleName, offset, pattern).FirstOrDefault();
    }

    public nuint Scan(Module module, uint offset, params string[] pattern)
    {
        return ScanAll(module, offset, pattern).FirstOrDefault();
    }

    public nuint Scan(Module module, uint offset, params byte[] pattern)
    {
        return ScanAll(module, offset, pattern).FirstOrDefault();
    }

    public nuint Scan(nuint startAddress, nuint endAddress, uint offset, params string[] pattern)
    {
        return ScanAll(startAddress, endAddress, offset, pattern).FirstOrDefault();
    }

    public nuint Scan(nuint startAddress, nuint endAddress, uint offset, params byte[] pattern)
    {
        return ScanAll(startAddress, endAddress, offset, pattern).FirstOrDefault();
    }

    public nuint Scan(nuint startAddress, uint size, uint offset, params string[] pattern)
    {
        return ScanAll(startAddress, size, offset, pattern).FirstOrDefault();
    }

    public nuint Scan(nuint startAddress, uint size, uint offset, params byte[] pattern)
    {
        return ScanAll(startAddress, size, offset, pattern).FirstOrDefault();
    }
}
