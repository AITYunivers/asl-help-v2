using System.Collections.Generic;
using System.Linq;

using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public nuint ScanPages(Signature signature, uint alignment = 1)
    {
        return ScanPagesAll(signature, false, alignment).FirstOrDefault();
    }

    public nuint ScanPages(Signature signature, bool allPages, uint alignment = 1)
    {
        return ScanPagesAll(signature, allPages, alignment).FirstOrDefault();
    }

    public IEnumerable<nuint> ScanPagesAll(Signature signature, uint alignment = 1)
    {
        return ScanPagesAll(signature, false, alignment);
    }

    public IEnumerable<nuint> ScanPagesAll(Signature signature, bool allPages, uint alignment = 1)
    {
        foreach (MemoryPage page in Pages(allPages))
        {
            foreach (nuint scanResult in ScanAll(signature, page.Base, page.RegionSize, alignment))
            {
                yield return scanResult;
            }
        }
    }
}
