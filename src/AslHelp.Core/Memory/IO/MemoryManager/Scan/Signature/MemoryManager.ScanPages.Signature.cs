namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
{
    public nint ScanPages(Signature signature, int alignment = 1)
    {
        return ScanPagesAll(false, signature, alignment).FirstOrDefault();
    }

    public nint ScanPages(bool allPages, Signature signature, int alignment = 1)
    {
        return ScanPagesAll(allPages, signature, alignment).FirstOrDefault();
    }

    public nint ScanPagesRel(Signature signature, int alignment = 1)
    {
        return ScanPagesAllRel(false, signature, alignment).FirstOrDefault();
    }

    public nint ScanPagesRel(bool allPages, Signature signature, int alignment = 1)
    {
        return ScanPagesAllRel(allPages, signature, alignment).FirstOrDefault();
    }

    public IEnumerable<nint> ScanPagesAll(Signature signature, int alignment = 1)
    {
        return ScanPagesAll(false, signature, alignment);
    }

    public IEnumerable<nint> ScanPagesAll(bool allPages, Signature signature, int alignment = 1)
    {

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
