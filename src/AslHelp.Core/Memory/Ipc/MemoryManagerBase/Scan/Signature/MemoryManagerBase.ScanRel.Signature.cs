using System.Linq;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint ScanRel(Signature signature, uint alignment = 1)
    {
        return ScanAllRel(signature, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, uint size, uint alignment = 1)
    {
        return ScanAllRel(signature, size, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, string moduleName, uint alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAllRel(signature, module, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, string moduleName, uint size, uint alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAllRel(signature, module, size, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, Module module, uint alignment = 1)
    {
        return ScanAllRel(signature, module, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, Module module, uint size, uint alignment = 1)
    {
        return ScanAllRel(signature, module, size, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, nuint startAddress, nuint endAddress, uint alignment = 1)
    {
        return ScanAllRel(signature, startAddress, endAddress, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, nuint startAddress, uint size, uint alignment = 1)
    {
        return ScanAllRel(signature, startAddress, size, alignment).FirstOrDefault();
    }
}
