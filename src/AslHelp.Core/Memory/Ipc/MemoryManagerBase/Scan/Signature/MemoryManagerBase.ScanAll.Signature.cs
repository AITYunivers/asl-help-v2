using System.Collections.Generic;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public IEnumerable<nuint> ScanAll(Signature signature, uint alignment = 1)
    {
        return ScanAll(signature, MainModule, alignment);
    }

    public IEnumerable<nuint> ScanAll(Signature signature, uint size, uint alignment = 1)
    {
        return ScanAll(signature, MainModule, size, alignment);
    }

    public IEnumerable<nuint> ScanAll(Signature signature, string moduleName, uint alignment = 1)
    {
        return ScanAll(signature, Modules[moduleName], alignment);
    }

    public IEnumerable<nuint> ScanAll(Signature signature, string moduleName, uint size, uint alignment = 1)
    {
        return ScanAll(signature, Modules[moduleName], size, alignment);
    }

    public IEnumerable<nuint> ScanAll(Signature signature, Module module, uint alignment = 1)
    {
        return ScanAll(signature, module.Base, module.MemorySize, alignment);
    }

    public IEnumerable<nuint> ScanAll(Signature signature, Module module, uint size, uint alignment = 1)
    {
        return ScanAll(signature, module.Base, size, alignment);
    }

    public IEnumerable<nuint> ScanAll(Signature signature, nuint startAddress, nuint endAddress, uint alignment = 1)
    {
        uint size = (uint)(endAddress - startAddress);
        return ScanAll(signature, startAddress, size, alignment);
    }

    public IEnumerable<nuint> ScanAll(Signature signature, nuint startAddress, uint size, uint alignment = 1)
    {
        if (size <= 0)
        {
            yield break;
        }

        byte[] memory = new byte[size];
        if (!TryReadSpan<byte>(memory, startAddress))
        {
            yield break;
        }

        foreach (uint scanOffset in new ScanEnumerator(memory, signature, alignment))
        {
            yield return startAddress + scanOffset + (uint)signature.Offset;
        }
    }
}
