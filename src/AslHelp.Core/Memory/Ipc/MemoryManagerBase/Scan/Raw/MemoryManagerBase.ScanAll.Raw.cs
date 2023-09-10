using System.Collections.Generic;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.SignatureScanning;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public IEnumerable<nuint> ScanAll(int offset, params string[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(signature);
    }

    public IEnumerable<nuint> ScanAll(int offset, params byte[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(signature);
    }

    public IEnumerable<nuint> ScanAll(string moduleName, int offset, params string[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(signature, moduleName);
    }

    public IEnumerable<nuint> ScanAll(string moduleName, int offset, params byte[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(signature, moduleName);
    }

    public IEnumerable<nuint> ScanAll(Module module, int offset, params string[] pattern)
    {
        return ScanAll(module.Base, module.MemorySize, offset, pattern);
    }

    public IEnumerable<nuint> ScanAll(Module module, int offset, params byte[] pattern)
    {
        return ScanAll(module.Base, module.MemorySize, offset, pattern);
    }

    public IEnumerable<nuint> ScanAll(nuint startAddress, nuint endAddress, int offset, params string[] pattern)
    {
        uint size = (uint)(endAddress - startAddress);
        return ScanAll(startAddress, size, offset, pattern);
    }

    public IEnumerable<nuint> ScanAll(nuint startAddress, nuint endAddress, int offset, params byte[] pattern)
    {
        uint size = (uint)(endAddress - startAddress);
        return ScanAll(startAddress, size, offset, pattern);
    }

    public IEnumerable<nuint> ScanAll(nuint startAddress, uint size, int offset, params string[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(signature, startAddress, size);
    }

    public IEnumerable<nuint> ScanAll(nuint startAddress, uint size, int offset, params byte[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(signature, startAddress, size);
    }

    public IEnumerable<uint> ScanAll(byte[] memory, int offset, params string[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanAll(signature, memory);
    }
}
