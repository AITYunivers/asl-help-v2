namespace AslHelp.Core.Memory;

internal static unsafe partial class WinApi
{
    /// <summary>
    ///     Closes an open object handle.
    ///     For further information, see:
    ///     <i><see href="https://learn.microsoft.com/en-us/windows/win32/api/handleapi/nf-handleapi-closehandle">CloseHandle function (handleapi.h)</see></i>.
    /// </summary>
    /// <param name="hObject">A handle that identifies the caller.</param>
    /// <returns>
    ///     A non-zero value if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern int CloseHandle(
        void* hObject);
}
