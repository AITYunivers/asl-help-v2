using System.Collections.Generic;
using System.Linq;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
{
    public nint ScanPages(int offset, params string[] pattern)
    {
        return ScanPagesAll(false, offset, pattern).FirstOrDefault();
    }

    public nint ScanPages(int offset, params byte[] pattern)
    {
        return ScanPagesAll(false, offset, pattern).FirstOrDefault();
    }

    public nint ScanPages(bool allPages, int offset, params string[] pattern)
    {
        return ScanPagesAll(allPages, offset, pattern).FirstOrDefault();
    }

    public nint ScanPages(bool allPages, int offset, params byte[] pattern)
    {
        return ScanPagesAll(allPages, offset, pattern).FirstOrDefault();
    }

    public nint ScanPagesRel(int offset, params string[] pattern)
    {
        return ScanPagesAllRel(false, offset, pattern).FirstOrDefault();
    }

    public nint ScanPagesRel(int offset, params byte[] pattern)
    {
        return ScanPagesAllRel(false, offset, pattern).FirstOrDefault();
    }

    public nint ScanPagesRel(bool allPages, int offset, params string[] pattern)
    {
        return ScanPagesAllRel(allPages, offset, pattern).FirstOrDefault();
    }

    public nint ScanPagesRel(bool allPages, int offset, params byte[] pattern)
    {
        return ScanPagesAllRel(allPages, offset, pattern).FirstOrDefault();
    }

    public IEnumerable<nint> ScanPagesAll(int offset, params string[] pattern)
    {
        return ScanPagesAll(false, offset, pattern);
    }

    public IEnumerable<nint> ScanPagesAll(int offset, params byte[] pattern)
    {
        return ScanPagesAll(false, offset, pattern);
    }

    public IEnumerable<nint> ScanPagesAll(bool allPages, int offset, params string[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanPagesAll(allPages, signature);
    }

    public IEnumerable<nint> ScanPagesAll(bool allPages, int offset, params byte[] pattern)
    {
        Signature signature = new(offset, pattern);
        return ScanPagesAll(allPages, signature);
    }

    public IEnumerable<nint> ScanPagesAllRel(int offset, params string[] pattern)
    {
        return ScanPagesAll(false, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanPagesAllRel(int offset, params byte[] pattern)
    {
        return ScanPagesAll(false, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanPagesAllRel(bool allPages, int offset, params string[] pattern)
    {
        return ScanPagesAll(allPages, offset, pattern).Select(FromAssemblyAddress);
    }

    public IEnumerable<nint> ScanPagesAllRel(bool allPages, int offset, params byte[] pattern)
    {
        return ScanPagesAll(allPages, offset, pattern).Select(FromAssemblyAddress);
    }
}
