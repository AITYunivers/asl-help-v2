﻿using System.Collections.Generic;

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
            string msg = "MainModule could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(module, signature, alignment);
    }

    public IEnumerable<nuint> ScanAll(uint size, Signature signature, uint alignment = 1)
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = "MainModule could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(module, size, signature, alignment);
    }

    public IEnumerable<nuint> ScanAll(string moduleName, Signature signature, uint alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(module, signature, alignment);
    }

    public IEnumerable<nuint> ScanAll(string moduleName, uint size, Signature signature, uint alignment = 1)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ScanAll(module, size, signature, alignment);
    }

    public IEnumerable<nuint> ScanAll(Module module, Signature signature, uint alignment = 1)
    {
        return ScanAll(module.Base, module.MemorySize, signature, alignment);
    }

    public IEnumerable<nuint> ScanAll(Module module, uint size, Signature signature, uint alignment = 1)
    {
        return ScanAll(module.Base, size, signature, alignment);
    }

    public IEnumerable<nuint> ScanAll(nuint startAddress, nuint endAddress, Signature signature, uint alignment = 1)
    {
        uint size = (uint)(endAddress - startAddress);
        return ScanAll(startAddress, size, signature, alignment);
    }

    public IEnumerable<nuint> ScanAll(nuint startAddress, uint size, Signature signature, uint alignment = 1)
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
