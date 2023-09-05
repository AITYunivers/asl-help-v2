using System.Linq;

using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint Scan(Signature signature, uint alignment = 1)
    {
        return ScanAll(signature, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, uint size, uint alignment = 1)
    {
        return ScanAll(signature, size, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, string moduleName, uint alignment = 1)
    {
        return ScanAll(signature, moduleName, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, string moduleName, uint size, uint alignment = 1)
    {
        return ScanAll(signature, moduleName, size, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, Module module, uint alignment = 1)
    {
        return ScanAll(signature, module, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, Module module, uint size, uint alignment = 1)
    {
        return ScanAll(signature, module, size, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, nuint startAddress, nuint endAddress, uint alignment = 1)
    {
        return ScanAll(signature, startAddress, endAddress, alignment).FirstOrDefault();
    }

    public nuint Scan(Signature signature, nuint startAddress, uint size, uint alignment = 1)
    {
        return ScanAll(signature, startAddress, size, alignment).FirstOrDefault();
    }
}
