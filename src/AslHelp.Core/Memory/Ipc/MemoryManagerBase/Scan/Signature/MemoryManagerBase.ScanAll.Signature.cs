using System.Collections.Generic;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public IEnumerable<nint> ScanAll(Signature signature, int alignment = 1)
    {
        return ScanAll(signature, MainModule, alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, int size, int alignment = 1)
    {
        return ScanAll(signature, MainModule, size, alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, string moduleName, int alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAll] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(signature, module, alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, string moduleName, int size, int alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAll] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(signature, module, size, alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, Module module, int alignment = 1)
    {
        return ScanAll(signature, module.Base, module.MemorySize, alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, Module module, int size, int alignment = 1)
    {
        return ScanAll(signature, module.Base, size, alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, nuint startAddress, nuint endAddress, int alignment = 1)
    {
        int size = (int)(endAddress - startAddress);
        return ScanAll(signature, startAddress, size, alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, nuint startAddress, int size, int alignment = 1)
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

        foreach (int scanOffset in new ScanEnumerator(memory, signature, alignment))
        {
            yield return startAddress + scanOffset + signature.Offset;
        }
    }
}
