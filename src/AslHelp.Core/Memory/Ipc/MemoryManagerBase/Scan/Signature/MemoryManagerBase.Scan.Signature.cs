using System.Linq;

using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint Scan(Signature signature, uint alignment = 1)
    {
        return ScanAll(signature, alignment).FirstOrDefault();
    }

    public nuint Scan(uint size, Signature signature, uint alignment = 1)
    {
        return ScanAll(size, signature, alignment).FirstOrDefault();
    }

    public nuint Scan(string moduleName, Signature signature, uint alignment = 1)
    {
        return ScanAll(moduleName, signature, alignment).FirstOrDefault();
    }

    public nuint Scan(string moduleName, uint size, Signature signature, uint alignment = 1)
    {
        return ScanAll(moduleName, size, signature, alignment).FirstOrDefault();
    }

    public nuint Scan(Module module, Signature signature, uint alignment = 1)
    {
        return ScanAll(module, signature, alignment).FirstOrDefault();
    }

    public nuint Scan(Module module, uint size, Signature signature, uint alignment = 1)
    {
        return ScanAll(module, size, signature, alignment).FirstOrDefault();
    }

    public nuint Scan(nuint startAddress, nuint endAddress, Signature signature, uint alignment = 1)
    {
        return ScanAll(startAddress, endAddress, signature, alignment).FirstOrDefault();
    }

    public nuint Scan(nuint startAddress, uint size, Signature signature, uint alignment = 1)
    {
        return ScanAll(startAddress, size, signature, alignment).FirstOrDefault();
    }
}
