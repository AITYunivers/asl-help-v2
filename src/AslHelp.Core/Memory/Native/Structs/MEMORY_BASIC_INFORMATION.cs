namespace AslHelp.Core.Memory;

/// <summary>
///     Contains information about a range of pages in the virtual address space of a process.
/// </summary>
/// <remarks>
///     For further information see:
///    <i><see href="https://docs.microsoft.com/windows/win32/api/winnt/ns-winnt-memory_basic_information">MEMORY_BASIC_INFORMATION structure (winnt.h)</see></i>.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
internal unsafe struct MEMORY_BASIC_INFORMATION
{
    /// <summary>
    ///     A pointer to the base address of the region of pages.
    /// </summary>
    public void* BaseAddress;

    /// <summary>
    ///     A pointer to the base address of a range of pages allocated by the VirtualAlloc function.
    /// </summary>
    public void* AllocationBase;

    /// <summary>
    ///     The memory protection option when the region was initially allocated.
    /// </summary>
    public MemProtect AllocationProtect;

    /// <summary>
    ///     The size of the region beginning at the base address in which all pages have identical attributes, in bytes.
    /// </summary>
    public nuint RegionSize;

    /// <summary>
    ///     The state of the pages in the region.
    /// </summary>
    public MemState State;

    /// <summary>
    ///     The access protection of the pages in the region.
    /// </summary>
    public MemProtect Protect;

    /// <summary>
    ///     The type of pages in the region.
    /// </summary>
    public MemType Type;

    public static nuint Size
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (nuint)sizeof(MEMORY_BASIC_INFORMATION);
    }
}

