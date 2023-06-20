using System.Collections.Generic;
using System.Linq;

using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint ScanPages(Signature signature, uint alignment = 1)
    {
        return ScanPagesAll(false, signature, alignment).FirstOrDefault();
    }

    public nuint ScanPages(bool allPages, Signature signature, uint alignment = 1)
    {
        return ScanPagesAll(allPages, signature, alignment).FirstOrDefault();
    }

    public nuint ScanPagesRel(Signature signature, uint alignment = 1)
    {
        return ScanPagesAllRel(false, signature, alignment).FirstOrDefault();
    }

    public nuint ScanPagesRel(bool allPages, Signature signature, uint alignment = 1)
    {
        return ScanPagesAllRel(allPages, signature, alignment).FirstOrDefault();
    }

    public IEnumerable<nuint> ScanPagesAll(Signature signature, uint alignment = 1)
    {
        return ScanPagesAll(false, signature, alignment);
    }

    public IEnumerable<nuint> ScanPagesAll(bool allPages, Signature signature, uint alignment = 1)
    {
        foreach (MemoryPage page in Pages(allPages))
        {
            foreach (nuint scanResult in ScanAll(signature, page.Base, page.RegionSize, alignment))
            {
                yield return scanResult;
            }
        }
    }

    public IEnumerable<nuint> ScanPagesAllRel(Signature signature, uint alignment = 1)
    {
        return ScanPagesAll(false, signature, alignment).Select(FromAssemblyAddress);
    }

    public IEnumerable<nuint> ScanPagesAllRel(bool allPages, Signature signature, uint alignment = 1)
    {
        return ScanPagesAll(allPages, signature, alignment).Select(FromAssemblyAddress);
    }
}
