﻿using System.Collections.Generic;
using System.Linq;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public IEnumerable<nint> ScanAllRel(int offset, params string[] pattern)
    {
        return ScanAll(offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(int offset, params byte[] pattern)
    {
        return ScanAll(offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(string moduleName, int offset, params string[] pattern)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAllRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(moduleName, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(string moduleName, int offset, params byte[] pattern)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAllRel] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(moduleName, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Module module, int offset, params string[] pattern)
    {
        return ScanAll(module, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(Module module, int offset, params byte[] pattern)
    {
        return ScanAll(module, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(nuint startAddress, nuint endAddress, int offset, params string[] pattern)
    {
        return ScanAll(startAddress, endAddress, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(nuint startAddress, nuint endAddress, int offset, params byte[] pattern)
    {
        return ScanAll(startAddress, endAddress, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(nuint startAddress, int size, int offset, params string[] pattern)
    {
        return ScanAll(startAddress, size, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanAllRel(nuint startAddress, int size, int offset, params byte[] pattern)
    {
        return ScanAll(startAddress, size, offset, pattern).Select(FromAssemblyAddress);
    }
}