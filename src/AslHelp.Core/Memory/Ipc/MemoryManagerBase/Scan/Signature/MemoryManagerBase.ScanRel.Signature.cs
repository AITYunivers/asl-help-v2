using System.Linq;

namespace AslHelp.Core.Memory.Ipc;

public abstract partial class MemoryManagerBase
{
    public nint ScanRel(Signature signature, int alignment = 1)
    {
        return ScanAllRel(signature, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, int size, int alignment = 1)
    {
        return ScanAllRel(signature, size, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, string moduleName, int alignment = 1)
    {
        return ScanAllRel(signature, moduleName, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, string moduleName, int size, int alignment = 1)
    {
        return ScanAllRel(signature, moduleName, size, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, Module module, int alignment = 1)
    {
        return ScanAllRel(signature, module, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, Module module, int size, int alignment = 1)
    {
        return ScanAllRel(signature, module, size, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, nint startAddress, nint endAddress, int alignment = 1)
    {
        return ScanAllRel(signature, startAddress, endAddress, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, nint startAddress, int size, int alignment = 1)
    {
        return ScanAllRel(signature, startAddress, size, alignment).FirstOrDefault();
    }
}
