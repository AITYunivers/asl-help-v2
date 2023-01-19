namespace AslHelp.Core.Memory;

internal static unsafe partial class Win32
{
    /// <summary>
    ///     Copies the data in the specified address range from the address space of
    ///     the specified process into the specified buffer of the current process.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/memoryapi/nf-memoryapi-readprocessmemory">ReadProcessMemory function (memoryapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A <see cref="Process.Handle"/> to the process with the memory that is being read.</param>
    /// <param name="lpBaseAddress">A pointer to the base address in the specified process from which to read.</param>
    /// <param name="lpBuffer">A pointer to a buffer that receives the contents from the address space of the specified process.</param>
    /// <param name="nSize">The number of bytes to be read from the specified process.</param>
    /// <param name="lpNumberOfBytesRead">The number of bytes transferred into the specified buffer.</param>
    /// <returns>
    ///     A non-zero value if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern int ReadProcessMemory(
        void* hProcess,
        void* lpBaseAddress,
        void* lpBuffer,
        nuint nSize,
        nuint* lpNumberOfBytesRead);

    /// <summary>
    ///     Writes data to an area of memory in a specified process.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/memoryapi/nf-memoryapi-writeprocessmemory">WriteProcessMemory function (memoryapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process memory to be modified.</param>
    /// <param name="lpBaseAddress">A pointer to the base address in the specified process to which data is written.</param>
    /// <param name="lpBuffer">A pointer to the buffer that contains data to be written in the address space of the specified process.</param>
    /// <param name="nSize">The number of bytes to be written to the specified process.</param>
    /// <param name="lpNumberOfBytesWritten">The number of bytes transferred into the specified process.</param>
    /// <returns>
    ///     A non-zero value if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern int WriteProcessMemory(
        void* hProcess,
        void* lpBaseAddress,
        void* lpBuffer,
        nuint nSize,
        nuint* lpNumberOfBytesWritten);

    /// <summary>
    ///     Retrieves information about a range of pages within the virtual address space of a specified process.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/memoryapi/nf-memoryapi-virtualqueryex">VirtualQueryEx function (memoryapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process whose memory information is queried.</param>
    /// <param name="lpAddress">A pointer to the base address of the region of pages to be queried.</param>
    /// <param name="lpBuffer">The <see cref="MEMORY_BASIC_INFORMATION"/> structure in which information about the specified page range is returned.</param>
    /// <param name="dwLength">The size of the buffer pointed to by <paramref name="lpBuffer"/>, in bytes.</param>
    /// <returns>
    ///     The actual number of bytes returned in the information buffer if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    private static extern nuint VirtualQueryEx(
        void* hProcess,
        void* lpAddress,
        MEMORY_BASIC_INFORMATION* lpBuffer,
        nuint dwLength);

    /// <summary>
    ///     Reserves, commits, or changes the state of a region of memory within the virtual address space of a specified process.<br/>
    ///     For further information see:
    ///     <i><see href="https://learn.microsoft.com/windows/win32/api/memoryapi/nf-memoryapi-virtualallocex">VirtualAllocEx function (memoryapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process within which the memory should be allocated.</param>
    /// <param name="lpAddress">The desired starting address for the region of memory to be allocated.</param>
    /// <param name="dwSize">The size of the region of memory to allocate, in bytes.</param>
    /// <param name="flAllocationType">The type of memory allocation.</param>
    /// <param name="flProtect">The memory protection for the region of pages to be allocated.</param>
    /// <returns>
    ///     The base address of the allocated region of pages if the function succeeds;
    ///     otherwise, <see langword="null"/>.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern void* VirtualAllocEx(
        void* hProcess,
        void* lpAddress,
        nuint dwSize,
        MemState flAllocationType,
        MemProtect flProtect);

    /// <summary>
    ///     Releases, decommits, or releases and decommits a region of memory within the virtual address space of a specified process.<br/>
    ///     For further information see:
    ///     <i><see href="https://learn.microsoft.com/windows/win32/api/memoryapi/nf-memoryapi-virtualfreeex">VirtualAllocEx function (memoryapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process within which the memory should be freed.</param>
    /// <param name="lpAddress">The starting address of the region of memory to be freed.</param>
    /// <param name="dwSize">The size of the region of memory to free, in bytes.</param>
    /// <param name="dwFreeType">The type of free operation.</param>
    /// <returns>
    ///     A non-zero value if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern int VirtualFreeEx(
        void* hProcess,
        void* lpAddress,
        nuint dwSize,
        MemState dwFreeType);
}
