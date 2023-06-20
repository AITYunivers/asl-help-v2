using System.Collections.Generic;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public IEnumerable<nuint> ScanAll(uint offset, params string[] pattern)
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = "[ScanAll] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(module, offset, pattern);
    }

    public IEnumerable<nuint> ScanAll(uint offset, params byte[] pattern)
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = "[ScanAll] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(module, offset, pattern);
    }

    public IEnumerable<nuint> ScanAll(string moduleName, uint offset, params string[] pattern)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAll] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(module, offset, pattern);
    }

    public IEnumerable<nuint> ScanAll(string moduleName, uint offset, params byte[] pattern)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAll] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(module, offset, pattern);
    }

    public IEnumerable<nuint> ScanAll(Module module, uint offset, params string[] pattern)
    {
        return ScanAll(module.Base, module.MemorySize, offset, pattern);
    }

    public IEnumerable<nuint> ScanAll(Module module, uint offset, params byte[] pattern)
    {
        return ScanAll(module.Base, module.MemorySize, offset, pattern);
    }

    public IEnumerable<nuint> ScanAll(nuint startAddress, nuint endAddress, uint offset, params string[] pattern)
    {
        uint size = (uint)(endAddress - startAddress);
        return ScanAll(startAddress, size, offset, pattern);
    }

    public IEnumerable<nuint> ScanAll(nuint startAddress, nuint endAddress, uint offset, params byte[] pattern)
    {
        uint size = (uint)(endAddress - startAddress);
        return ScanAll(startAddress, size, offset, pattern);
    }

    public IEnumerable<nuint> ScanAll(nuint startAddress, uint size, uint offset, params string[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(signature, startAddress, size);
    }

    public IEnumerable<nuint> ScanAll(nuint startAddress, uint size, uint offset, params byte[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(signature, startAddress, size);
    }
}
