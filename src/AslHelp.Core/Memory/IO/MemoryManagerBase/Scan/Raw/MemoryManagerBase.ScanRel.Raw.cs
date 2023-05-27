using System.Linq;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
{
    public nint ScanRel(int offset, params string[] pattern)
    {
        return ScanAllRel(offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(int offset, params byte[] pattern)
    {
        return ScanAllRel(offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(string moduleName, int offset, params string[] pattern)
    {
        return ScanAllRel(moduleName, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(string moduleName, int offset, params byte[] pattern)
    {
        return ScanAllRel(moduleName, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(Module module, int offset, params string[] pattern)
    {
        return ScanAllRel(module, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(Module module, int offset, params byte[] pattern)
    {
        return ScanAllRel(module, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(nint startAddress, nint endAddress, int offset, params string[] pattern)
    {
        return ScanAllRel(startAddress, endAddress, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(nint startAddress, nint endAddress, int offset, params byte[] pattern)
    {
        return ScanAllRel(startAddress, endAddress, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(nint startAddress, int size, int offset, params string[] pattern)
    {
        return ScanAllRel(startAddress, size, offset, pattern).FirstOrDefault();
    }

    public nint ScanRel(nint startAddress, int size, int offset, params byte[] pattern)
    {
        return ScanAllRel(startAddress, size, offset, pattern).FirstOrDefault();
    }
}
