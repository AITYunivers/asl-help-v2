﻿using System.Linq;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManager
{
    public nuint Scan(int offset, params string[] pattern)
    {
        return ScanAll(offset, pattern).FirstOrDefault();
    }

    public nuint Scan(string moduleName, int offset, params string[] pattern)
    {
        return ScanAll(moduleName, offset, pattern).FirstOrDefault();
    }

    public nuint Scan(Module module, int offset, params string[] pattern)
    {
        return ScanAll(module, offset, pattern).FirstOrDefault();
    }

    public nuint Scan(nuint startAddress, nuint endAddress, int offset, params string[] pattern)
    {
        return ScanAll(startAddress, endAddress, offset, pattern).FirstOrDefault();
    }

    public nuint Scan(nuint startAddress, uint size, int offset, params string[] pattern)
    {
        return ScanAll(startAddress, size, offset, pattern).FirstOrDefault();
    }

    public uint Scan(byte[] memory, int offset, params string[] pattern)
    {
        return ScanAll(memory, offset, pattern).FirstOrDefault();
    }
}
