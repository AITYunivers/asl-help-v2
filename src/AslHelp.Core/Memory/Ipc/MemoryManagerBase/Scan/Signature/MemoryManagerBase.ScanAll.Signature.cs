using System.Collections.Generic;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public IEnumerable<nuint> ScanAll(Signature signature, uint alignment = 1)
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = "[ScanAll] MainModule could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(signature, module, alignment);
    }

    public IEnumerable<nuint> ScanAll(Signature signature, uint size, uint alignment = 1)
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = "[ScanAll] MainModule could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(signature, module, size, alignment);
    }

    public IEnumerable<nuint> ScanAll(Signature signature, string moduleName, uint alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAll] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(signature, module, alignment);
    }

    public IEnumerable<nuint> ScanAll(Signature signature, string moduleName, uint size, uint alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ScanAll] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(signature, module, size, alignment);
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
            yield return startAddress + scanOffset + signature.Offset;
        }
    }
}
