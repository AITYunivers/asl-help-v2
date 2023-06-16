using System;
using System.Linq;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint ScanRel(Signature signature, int alignment = 1)
    {
        return ScanAllRel(signature, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, int size, int alignment = 1)
    {
        return ScanAllRel(signature, size, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, string moduleName, int alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAllRel(signature, module, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, string moduleName, int size, int alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAllRel(signature, module, size, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, Module module, int alignment = 1)
    {
        return ScanAllRel(signature, module, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, Module module, int size, int alignment = 1)
    {
        return ScanAllRel(signature, module, size, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, nuint startAddress, nuint endAddress, int alignment = 1)
    {
        return ScanAllRel(signature, startAddress, endAddress, alignment).FirstOrDefault();
    }

    public nuint ScanRel(Signature signature, nuint startAddress, int size, int alignment = 1)
    {
        return ScanAllRel(signature, startAddress, size, alignment).FirstOrDefault();
    }
}
