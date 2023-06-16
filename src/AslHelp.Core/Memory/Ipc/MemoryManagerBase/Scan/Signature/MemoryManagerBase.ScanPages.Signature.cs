using System.Collections.Generic;
using System.Linq;

using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint ScanPages(Signature signature, int alignment = 1)
    {
        return ScanPagesAll(false, signature, alignment).FirstOrDefault();
    }

    public nuint ScanPages(bool allPages, Signature signature, int alignment = 1)
    {
        return ScanPagesAll(allPages, signature, alignment).FirstOrDefault();
    }

    public nuint ScanPagesRel(Signature signature, int alignment = 1)
    {
        return ScanPagesAllRel(false, signature, alignment).FirstOrDefault();
    }

    public nuint ScanPagesRel(bool allPages, Signature signature, int alignment = 1)
    {
        return ScanPagesAllRel(allPages, signature, alignment).FirstOrDefault();
    }

    public IEnumerable<nint> ScanPagesAll(Signature signature, int alignment = 1)
    {
        return ScanPagesAll(false, signature, alignment);
    }

    public IEnumerable<nint> ScanPagesAll(bool allPages, Signature signature, int alignment = 1)
    {
        foreach (MemoryPage page in Pages(allPages))
        {
            foreach (nuint scanResult in ScanAll(signature, page.Base, page.RegionSize))
            {
                yield return scanResult;
            }
        }
    }

    public IEnumerable<nint> ScanPagesAllRel(Signature signature, int alignment = 1)
    {
        return ScanPagesAll(false, signature, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanPagesAllRel(bool allPages, Signature signature, int alignment = 1)
    {
        return ScanPagesAll(false, signature, alignment).Select(FromAssemblyAddress);
    }
}
