namespace AslHelp.Core.Memory;

public readonly record struct MemoryPage
{
    internal unsafe MemoryPage(MEMORY_BASIC_INFORMATION mbi)
    {
        Base = (nuint)mbi.BaseAddress;
        RegionSize = (uint)mbi.RegionSize;
        Protect = mbi.Protect;
        State = mbi.State;
        Type = mbi.Type;
    }

    public nuint Base { get; }
    public uint RegionSize { get; }

    public MemProtect Protect { get; }
    public MemState State { get; }
    public MemType Type { get; }
}
