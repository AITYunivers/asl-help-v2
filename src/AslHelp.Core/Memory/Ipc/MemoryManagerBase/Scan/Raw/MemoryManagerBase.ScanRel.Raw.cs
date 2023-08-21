using System.Linq;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint ScanRel(int offset, params string[] pattern)
    {
        return ScanAllRel(offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(int offset, params byte[] pattern)
    {
        return ScanAllRel(offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(string moduleName, int offset, params string[] pattern)
    {
        return ScanAllRel(moduleName, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(string moduleName, int offset, params byte[] pattern)
    {
        return ScanAllRel(moduleName, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(Module module, int offset, params string[] pattern)
    {
        return ScanAllRel(module, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(Module module, int offset, params byte[] pattern)
    {
        return ScanAllRel(module, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(nuint startAddress, nuint endAddress, int offset, params string[] pattern)
    {
        return ScanAllRel(startAddress, endAddress, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(nuint startAddress, nuint endAddress, int offset, params byte[] pattern)
    {
        return ScanAllRel(startAddress, endAddress, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(nuint startAddress, uint size, int offset, params string[] pattern)
    {
        return ScanAllRel(startAddress, size, offset, pattern).FirstOrDefault();
    }

    public nuint ScanRel(nuint startAddress, uint size, int offset, params byte[] pattern)
    {
        return ScanAllRel(startAddress, size, offset, pattern).FirstOrDefault();
    }
}
