namespace AslHelp.Core.Memory.Native.Enums;

/// <summary>
///     Provides memory page type constants.
/// </summary>
/// <remarks>
///     For further information see:
///     <i><see href="https://docs.microsoft.com/windows/win32/api/winnt/ns-winnt-memory_basic_information#members">MEMORY_BASIC_INFORMATION structure (winnt.h)</see></i>
/// </remarks>
public enum MemType : uint
{
    /// <summary>
    ///     Indicates that the memory pages within the region are private.
    /// </summary>
    MEM_PRIVATE = 0x0020000,

    /// <summary>
    ///     Indicates that the memory pages within the region are mapped into the view of a section.
    /// </summary>
    MEM_MAPPED = 0x0040000,

    /// <summary>
    ///     Indicates that the memory pages within the region are mapped into the view of an image section.
    /// </summary>
    MEM_IMAGE = 0x1000000
}
