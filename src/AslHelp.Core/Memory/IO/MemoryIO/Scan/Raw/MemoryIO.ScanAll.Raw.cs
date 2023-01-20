﻿namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryIO
{
    public IEnumerable<nint> ScanAll(int offset, params string[] pattern)
    {
        return ScanAll(MainModule, offset, pattern);
    }

    public IEnumerable<nint> ScanAll(int offset, params byte[] pattern)
    {
        return ScanAll(MainModule, offset, pattern);
    }

    public IEnumerable<nint> ScanAll(string module, int offset, params string[] pattern)
    {
        return ScanAll(Modules[module], offset, pattern);
    }

    public IEnumerable<nint> ScanAll(string module, int offset, params byte[] pattern)
    {
        return ScanAll(Modules[module], offset, pattern);
    }

    public IEnumerable<nint> ScanAll(Module module, int offset, params string[] pattern)
    {
        if (module is null)
        {
            Debug.Warn("[Scan] Module could not be found.");

            return Enumerable.Empty<nint>();
        }

        return ScanAll(module.Base, module.MemorySize, offset, pattern);
    }

    public IEnumerable<nint> ScanAll(Module module, int offset, params byte[] pattern)
    {
        if (module is null)
        {
            Debug.Warn("[Scan] Module could not be found.");

            return Enumerable.Empty<nint>();
        }

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

    }

    public IEnumerable<nint> ScanAll(nint startAddress, int size, int offset, params byte[] pattern)
    {

    }
}