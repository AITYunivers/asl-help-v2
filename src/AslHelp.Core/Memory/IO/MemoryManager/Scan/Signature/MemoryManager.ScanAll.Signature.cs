using AslHelp.Core.Memory.Signatures;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
{
    public IEnumerable<nint> ScanAll(Signature signature, int alignment = 1)
    {
        return ScanAll(signature, MainModule, alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, int size, int alignment = 1)
    {
        return ScanAll(signature, MainModule, size, alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, string module, int alignment = 1)
    {
        return ScanAll(signature, Modules[module], alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, string module, int size, int alignment = 1)
    {
        return ScanAll(signature, Modules[module], size, alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, Module module, int alignment = 1)
    {
        if (module is null)
        {
            Debug.Warn("[Scan] Module could not be found.");

            return Enumerable.Empty<nint>();
        }

        return ScanAll(signature, module.Base, module.MemorySize, alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, Module module, int size, int alignment = 1)
    {
        if (module is null)
        {
            Debug.Warn("[Scan] Module could not be found.");

            return Enumerable.Empty<nint>();
        }

        return ScanAll(signature, module.Base, size, alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, nint startAddress, nint endAddress, int alignment = 1)
    {
        int size = (int)(endAddress - startAddress);
        return ScanAll(signature, startAddress, size, alignment);
    }

    public IEnumerable<nint> ScanAll(Signature signature, nint startAddress, int size, int alignment = 1)
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

        foreach (int offset in new ScanEnumerator(memory, signature, alignment))
        {
            yield return startAddress + offset + signature.Offset;
        }
    }
}
