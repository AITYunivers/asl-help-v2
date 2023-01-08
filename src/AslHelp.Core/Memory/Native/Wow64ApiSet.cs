namespace AslHelp.Core.Memory;

#pragma warning disable IDE1006
internal static unsafe partial class Native
{
    /// <summary>
    ///     Determines whether the specified process is running under WOW64 or an Intel64 of x64 processor.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/wow64apiset/nf-wow64apiset-iswow64process">IsWow64Process function on Microsoft docs</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process.</param>
    /// <param name="Wow64Process">
    ///     Specifies whether the process is running under WOW64 on an Intel64 or x64 processor.
    ///     <para>
    ///     If the process is running under 32-bit Windows, the value is set to <see langword="false"/>.<br/>
    ///     If the process is a 32-bit application running under 64-bit Windows 10 on ARM, the value is set to <see langword="false"/>.<br/>
    ///     If the process is a 64-bit application running under 64-bit Windows, the value is also set to <see langword="false"/>.
    ///     </para>
    /// </param>
    /// <returns>
    ///     A non-zero value if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    private static extern int IsWow64Process(
        nint hProces,
        int* Wow64Process);
}
#pragma warning restore IDE1006
