using System.Collections.Generic;
using System.Linq;

using AslHelp.Core.Memory.SignatureScanning;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManager
{
    public nuint ScanPages(int offset, params string[] pattern)
    {
        return ScanPagesAll(false, offset, pattern).FirstOrDefault();
    }

    public nuint ScanPages(bool allPages, int offset, params string[] pattern)
    {
        return ScanPagesAll(allPages, offset, pattern).FirstOrDefault();
    }

    public IEnumerable<nuint> ScanPagesAll(int offset, params string[] pattern)
    {
        return ScanPagesAll(false, offset, pattern);
    }

    public IEnumerable<nuint> ScanPagesAll(bool allPages, int offset, params string[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanPagesAll(signature, allPages);
    }
}
