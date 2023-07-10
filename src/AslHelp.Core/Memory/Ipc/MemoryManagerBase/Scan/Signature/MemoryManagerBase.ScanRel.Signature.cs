using System.Linq;

using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint ScanRel(Signature signature, uint alignment = 1)
    {
        return ScanAllRel(signature, alignment).FirstOrDefault();
    }

    public nuint ScanRel(uint size, Signature signature, uint alignment = 1)
    {
        return ScanAllRel(size, signature, alignment).FirstOrDefault();
    }

    public nuint ScanRel(string moduleName, Signature signature, uint alignment = 1)
    {
        return ScanAllRel(moduleName, signature, alignment).FirstOrDefault();
    }

    public nuint ScanRel(string moduleName, uint size, Signature signature, uint alignment = 1)
    {
        return ScanAllRel(moduleName, size, signature, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Module module, Signature signature, uint alignment = 1)
    {
        return ScanAllRel(module, signature, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Module module, uint size, Signature signature, uint alignment = 1)
    {
        return ScanAllRel(module, size, signature, alignment).FirstOrDefault();
    }

    public nuint ScanRel(nuint startAddress, nuint endAddress, Signature signature, uint alignment = 1)
    {
        return ScanAllRel(startAddress, endAddress, signature, alignment).FirstOrDefault();
    }

    public nuint ScanRel(nuint startAddress, uint size, Signature signature, uint alignment = 1)
    {
        return ScanAllRel(startAddress, size, signature, alignment).FirstOrDefault();
    }
}
