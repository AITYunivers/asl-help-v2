namespace AslHelp.Core.Memory.Native.Enums;

/// <summary>
///     Provides memory page state constants.
/// </summary>
/// <remarks>
///     For further information see:
///     <i><see href="https://docs.microsoft.com/windows/win32/memory/memory-protection-constants#constants">Memory Protection Constants</see></i>
/// </remarks>
public enum MemState : uint
{
    /// <summary>
    ///     Allocates memory charges for the specified reserved memory pages.
    /// </summary>
    MEM_COMMIT = 0x00001000,

    /// <summary>
    ///     Reserves a range of the process' virtual address space.
    /// </summary>
    MEM_RESERVE = 0x00002000,

    /// <summary>
    ///     Decommits a range of the process' virtual address space.
    /// </summary>
    MEM_DECOMMIT = 0x00004000,

    /// <summary>
    ///     Releases a range of the process' virtual address space.
    /// </summary>
    MEM_RELEASE = 0x00008000,

    /// <summary>
    ///     Indicates that data in the memory range is no longer of interest.
    /// </summary>
    MEM_RESET = 0x00080000,

    /// <summary>
    ///     Allocates memory at the highest possible address.
    /// </summary>
    MEM_TOP_DOWN = 0x00100000,

    /// <summary>
    ///     Causes the system to track pages that are written to in the allocated region.
    /// </summary>
    MEM_WRITE_WATCH = 0x00200000,

    /// <summary>
    ///     Reserves an address range that can be used to map Address Windowing Extensions pages.
    /// </summary>
    MEM_PHYSICAL = 0x00400000,

    /// <summary>
    ///     Indicates that the data in the specified memory range is of interest to the caller and attempts to reverse the effects of <see cref="MEM_RESET"/>.
    /// </summary>
    MEM_RESET_UNDO = 0x01000000,

    /// <summary>
    ///     Allocates memory using large page support.
    /// </summary>
    MEM_LARGE_PAGES = 0x20000000
}

