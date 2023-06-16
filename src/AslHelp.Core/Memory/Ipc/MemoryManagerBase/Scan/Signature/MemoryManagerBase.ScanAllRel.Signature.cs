using System.Collections.Generic;
using System.Linq;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public IEnumerable<nint> ScanAllRel(Signature signature, int alignment = 1)
    {
        return ScanAll(signature, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, int size, int alignment = 1)
    {
        return ScanAll(signature, size, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, string moduleName, int alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAllRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(signature, module, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, string moduleName, int size, int alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAllRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(signature, module, size, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, Module module, int alignment = 1)
    {
        return ScanAll(signature, module, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, Module module, int size, int alignment = 1)
    {
        return ScanAll(signature, module, size, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, nuint startAddress, nuint endAddress, int alignment = 1)
    {
        return ScanAll(signature, startAddress, endAddress, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Signature signature, nuint startAddress, int size, int alignment = 1)
    {
        return ScanAll(signature, startAddress, size, alignment).Select(FromAssemblyAddress);
    }
}
