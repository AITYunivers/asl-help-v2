﻿namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryIO
{
    public nint ScanRel(Signature signature, int alignment = 1)
    {
        return ScanAllRel(signature, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, int size, int alignment = 1)
    {
        return ScanAllRel(signature, size, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, string module, int alignment = 1)
    {
        return ScanAllRel(signature, module, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, string module, int size, int alignment = 1)
    {
        return ScanAllRel(signature, module, size, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, Module module, int alignment = 1)
    {
        return ScanAllRel(signature, module, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, Module module, int size, int alignment = 1)
    {
        return ScanAllRel(signature, module, size, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, nint startAddress, nint endAddress, int alignment = 1)
    {
        return ScanAllRel(signature, startAddress, endAddress, alignment).FirstOrDefault();
    }

    public nint ScanRel(Signature signature, nint startAddress, int size, int alignment = 1)
    {
        return ScanAllRel(signature, startAddress, size, alignment).FirstOrDefault();
    }
}
