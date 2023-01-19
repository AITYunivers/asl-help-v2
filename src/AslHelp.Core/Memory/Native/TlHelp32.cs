namespace AslHelp.Core.Memory;

internal static unsafe partial class Native
{
    /// <summary>
    ///     Takes a snapshot of the specified processes, as well as the heaps, modules, and threads used by these processes.<br/>
    ///     For further information see:
    ///     <i><see href="https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-createtoolhelp32snapshot">CreateToolhelp32Snapshot function (tlhelp32.h)</see></i>.
    /// </summary>
    /// <param name="dwFlags">The portions of the system to be included in the snapshot.</param>
    /// <param name="th32ProcessID">The process identifier of the process to be included in the snapshot.</param>
    /// <returns>
    ///     An open handle to the specified snapshot if the funcion succeeds;
    ///     otherwise, <see langword="null"/>.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern void* CreateToolhelp32Snapshot(
        ThFlags dwFlags,
        uint th32ProcessID);

    /// <summary>
    ///     Retrieves information about the first module associated with a process or thread.<br/>
    ///     For further information see:
    ///     <i><see href="https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-module32firstw">Module32FirstW function (dbghelp.h)</see></i>.
    /// </summary>
    /// <param name="hSnapshot">A handle to the snapshot returned from a previous call to the <see cref="CreateToolhelp32Snapshot(uint, uint)"/> function.</param>
    /// <param name="lpme">A pointer to a <see cref="MODULEENTRY32W"/> structure.</param>
    /// <returns>
    ///     A non-zero value if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern int Module32FirstW(
        void* hSnapshot,
        MODULEENTRY32W* lpme);

    /// <summary>
    ///     Retrieves information about the next module associated with a process or thread.<br/>
    ///     For further information see:
    ///     <i><see href="https://learn.microsoft.com/en-us/windows/win32/api/tlhelp32/nf-tlhelp32-module32nextw">Module32NextW function (dbghelp.h)</see></i>.
    /// </summary>
    /// <param name="hSnapshot">A handle to the snapshot returned from a previous call to the <see cref="CreateToolhelp32Snapshot(uint, uint)"/> function.</param>
    /// <param name="lpme">A pointer to a <see cref="MODULEENTRY32W"/> structure.</param>
    /// <returns>
    ///     A non-zero value if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern int Module32NextW(
        void* hSnapshot,
        MODULEENTRY32W* lpme);
}
