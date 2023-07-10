using System.Collections.Generic;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.SignatureScanning;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public IEnumerable<nuint> ScanAll(uint offset, params string[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(signature);
    }

    public IEnumerable<nuint> ScanAll(uint offset, params byte[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(signature);
    }

    public IEnumerable<nuint> ScanAll(string moduleName, uint offset, params string[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(moduleName, signature);
    }

    public IEnumerable<nuint> ScanAll(string moduleName, uint offset, params byte[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(moduleName, signature);
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
        return ScanAll(startAddress, size, signature);
    }

    public IEnumerable<nuint> ScanAll(nuint startAddress, uint size, uint offset, params byte[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(startAddress, size, signature);
    }
}
