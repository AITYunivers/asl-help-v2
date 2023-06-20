﻿using System.Collections.Generic;
using System.Linq;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public IEnumerable<nuint> ScanAllRel(uint offset, params string[] pattern)
    {
        return ScanAll(offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(uint offset, params byte[] pattern)
    {
        return ScanAll(offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(string moduleName, uint offset, params string[] pattern)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAllRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(moduleName, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(string moduleName, uint offset, params byte[] pattern)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAllRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(moduleName, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(Module module, uint offset, params string[] pattern)
    {
        return ScanAll(module, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(Module module, uint offset, params byte[] pattern)
    {
        return ScanAll(module, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(nuint startAddress, nuint endAddress, uint offset, params string[] pattern)
    {
        return ScanAll(startAddress, endAddress, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(nuint startAddress, nuint endAddress, uint offset, params byte[] pattern)
    {
        return ScanAll(startAddress, endAddress, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(nuint startAddress, uint size, uint offset, params string[] pattern)
    {
        return ScanAll(startAddress, size, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanAllRel(nuint startAddress, uint size, uint offset, params byte[] pattern)
    {
        return ScanAll(startAddress, size, offset, pattern).Select(FromAssemblyAddress);
    }
}
