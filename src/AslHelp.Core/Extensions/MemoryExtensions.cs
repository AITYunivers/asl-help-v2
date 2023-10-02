using System.Linq;

using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Extensions;

public static class MemoryExtensions
{
    public static nuint Scan(this IMemoryManager memory, Signature[] signatures, uint alignment = 1)
    {
        return memory.Scan(signatures, memory.MainModule, alignment);
    }

    public static nuint Scan(this IMemoryManager memory, Signature[] signatures, uint size, uint alignment = 1)
    {
        return memory.Scan(signatures, memory.MainModule, size, alignment);
    }

    public static nuint Scan(this IMemoryManager memory, Signature[] signatures, string moduleName, uint alignment = 1)
    {
        return memory.Scan(signatures, memory.Modules[moduleName], alignment);
    }

    public static nuint Scan(this IMemoryManager memory, Signature[] signatures, string moduleName, uint size, uint alignment = 1)
    {
        return memory.Scan(signatures, memory.Modules[moduleName], size, alignment);
    }

    public static nuint Scan(this IMemoryManager memory, Signature[] signatures, Module module, uint alignment = 1)
    {
        return memory.Scan(signatures, module.Base, module.MemorySize, alignment);
    }

    public static nuint Scan(this IMemoryManager memory, Signature[] signatures, Module module, uint size, uint alignment = 1)
    {
        return memory.Scan(signatures, module.Base, size, alignment);
    }

    public static nuint Scan(this IMemoryManager memory, Signature[] signatures, nuint startAddress, nuint endAddress, uint alignment = 1)
    {
        uint size = (uint)(endAddress - startAddress);
        return memory.Scan(signatures, startAddress, size, alignment);
    }

    public static nuint Scan(this IMemoryManager memory, Signature[] signatures, nuint startAddress, uint size, uint alignment = 1)
    {
        if (size <= 0)
        {
            return 0;
        }

        byte[] buffer = new byte[size];
        if (!memory.TryReadSpan<byte>(buffer, startAddress))
        {
            return 0;
        }

        foreach (Signature signature in signatures)
        {
            foreach (uint scanOffset in new ScanEnumerator(buffer, signature, alignment))
            {
                return startAddress + (uint)(scanOffset + signature.Offset);
            }
        }

        return 0;
    }
}
