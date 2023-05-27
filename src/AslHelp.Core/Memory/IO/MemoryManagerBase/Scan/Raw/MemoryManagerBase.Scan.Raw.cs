using System.Linq;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
{
    public nint Scan(int offset, params string[] pattern)
    {
        return ScanAll(offset, pattern).FirstOrDefault();
    }

    public nint Scan(int offset, params byte[] pattern)
    {
        return ScanAll(offset, pattern).FirstOrDefault();
    }

    public nint Scan(string moduleName, int offset, params string[] pattern)
    {
        return ScanAll(moduleName, offset, pattern).FirstOrDefault();
    }

    public nint Scan(string moduleName, int offset, params byte[] pattern)
    {
        return ScanAll(moduleName, offset, pattern).FirstOrDefault();
    }

    public nint Scan(Module module, int offset, params string[] pattern)
    {
        return ScanAll(module, offset, pattern).FirstOrDefault();
    }

    public nint Scan(Module module, int offset, params byte[] pattern)
    {
        return ScanAll(module, offset, pattern).FirstOrDefault();
    }

    public nint Scan(nint startAddress, nint endAddress, int offset, params string[] pattern)
    {
        return ScanAll(startAddress, endAddress, offset, pattern).FirstOrDefault();
    }

    public nint Scan(nint startAddress, nint endAddress, int offset, params byte[] pattern)
    {
        return ScanAll(startAddress, endAddress, offset, pattern).FirstOrDefault();
    }

    public nint Scan(nint startAddress, int size, int offset, params string[] pattern)
    {
        return ScanAll(startAddress, size, offset, pattern).FirstOrDefault();
    }

    public nint Scan(nint startAddress, int size, int offset, params byte[] pattern)
    {
        return ScanAll(startAddress, size, offset, pattern).FirstOrDefault();
    }
}
