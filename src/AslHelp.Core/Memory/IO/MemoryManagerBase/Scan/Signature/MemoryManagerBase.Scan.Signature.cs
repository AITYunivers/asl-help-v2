﻿using System.Linq;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
{
    public nint Scan(Signature signature, int alignment = 1)
    {
        return ScanAll(signature, alignment).FirstOrDefault();
    }

    public nint Scan(Signature signature, int size, int alignment = 1)
    {
        return ScanAll(signature, size, alignment).FirstOrDefault();
    }

    public nint Scan(Signature signature, string moduleName, int alignment = 1)
    {
        return ScanAll(signature, moduleName, alignment).FirstOrDefault();
    }

    public nint Scan(Signature signature, string moduleName, int size, int alignment = 1)
    {
        return ScanAll(signature, moduleName, size, alignment).FirstOrDefault();
    }

    public nint Scan(Signature signature, Module module, int alignment = 1)
    {
        return ScanAll(signature, module, alignment).FirstOrDefault();
    }

    public nint Scan(Signature signature, Module module, int size, int alignment = 1)
    {
        return ScanAll(signature, module, size, alignment).FirstOrDefault();
    }

    public nint Scan(Signature signature, nint startAddress, nint endAddress, int alignment = 1)
    {
        return ScanAll(signature, startAddress, endAddress, alignment).FirstOrDefault();
    }

    public nint Scan(Signature signature, nint startAddress, int size, int alignment = 1)
    {
        return ScanAll(signature, startAddress, size, alignment).FirstOrDefault();
    }
}
