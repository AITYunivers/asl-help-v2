namespace AslHelp.Core.Memory;

internal static unsafe partial class Native
{
    /// <summary>
    ///     Retrieves the termination status of the specified thread.<br/>
    ///     For further information see:
    ///     <i><see href="https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getexitcodethread">GetExitCodeThread function (processthreadsapi.h)</see></i>.
    /// </summary>
    /// <param name="hThread">A handle to the thread.</param>
    /// <param name="lpExitCode">A pointer to a variable to receive the thread termination status.</param>
    /// <returns>
    ///     A non-zero value if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern int GetExitCodeThread(
        void* hThread,
        uint* lpExitCode);

    /// <summary>
    ///     Creates a thread that runs in the virtual address space of another process.<br/>
    ///     For further information see:
    ///     <i><see href="https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-createremotethread">CreateRemoteThread function (processthreadsapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process in which the thread is to be created.</param>
    /// <param name="lpThreadAttributes">Unused.</param>
    /// <param name="dwStackSize">The initial size of the stack, in bytes.</param>
    /// <param name="lpStartAddress">The starting address of the thread in the remote process.</param>
    /// <param name="lpParameter">A pointer to a variable to be passed to the thread function.</param>
    /// <param name="dwCreationFlags">The flags that control the creation of the thread.</param>
    /// <param name="lpThreadId">Unused.</param>
    /// <returns>
    ///     A handle to the new thread if function succeeds,
    ///     otherwise, <see langword="null"/>.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern void* CreateRemoteThread(
        void* hProcess,
        void* lpThreadAttributes,
        nuint dwStackSize,
        void* lpStartAddress,
        void* lpParameter,
        uint dwCreationFlags,
        uint* lpThreadId);
}
