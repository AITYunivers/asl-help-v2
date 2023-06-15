using System.Collections.Generic;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public IEnumerable<nint> ScanAll(int offset, params string[] pattern)
    {
        return ScanAll(MainModule, offset, pattern);
    }

    public IEnumerable<nint> ScanAll(int offset, params byte[] pattern)
    {
        return ScanAll(MainModule, offset, pattern);
    }

    public IEnumerable<nint> ScanAll(string moduleName, int offset, params string[] pattern)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAll] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(module, offset, pattern);
    }

    public IEnumerable<nint> ScanAll(string moduleName, int offset, params byte[] pattern)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAll] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(module, offset, pattern);
    }

    public IEnumerable<nint> ScanAll(Module module, int offset, params string[] pattern)
    {
        return ScanAll(module.Base, module.MemorySize, offset, pattern);
    }

    public IEnumerable<nint> ScanAll(Module module, int offset, params byte[] pattern)
    {
        return ScanAll(module.Base, module.MemorySize, offset, pattern);
    }

    public IEnumerable<nint> ScanAll(nint startAddress, nint endAddress, int offset, params string[] pattern)
    {
        int size = (int)(endAddress - startAddress);
        return ScanAll(startAddress, size, offset, pattern);
    }

    public IEnumerable<nint> ScanAll(nint startAddress, nint endAddress, int offset, params byte[] pattern)
    {
        int size = (int)(endAddress - startAddress);
        return ScanAll(startAddress, size, offset, pattern);
    }

    public IEnumerable<nint> ScanAll(nint startAddress, int size, int offset, params string[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(signature, startAddress, size);
    }

    public IEnumerable<nint> ScanAll(nint startAddress, int size, int offset, params byte[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(signature, startAddress, size);
    }
}
