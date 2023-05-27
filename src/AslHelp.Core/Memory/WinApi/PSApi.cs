using System.Runtime.InteropServices;

namespace AslHelp.Core.Memory;

internal static unsafe partial class WinApi
{
    /// <summary>
    ///     Retrieves a handle for each module in the specified process that meets the specified filter criteria.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/psapi/nf-psapi-enumprocessmodulesex">EnumProcessModulesEx function (psapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process.</param>
    /// <param name="lphModule">An array that receives the list of module handles.</param>
    /// <param name="cb">The size of the lphModule array, in bytes.</param>
    /// <param name="lpcbNeeded">The number of bytes required to store all module handles in the lphModule array.</param>
    /// <param name="dwFilterFlag">The filter criteria.</param>
    /// <returns>
    ///     A non-zero value if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern int K32EnumProcessModulesEx(
        void* hProcess,
        void** lphModule,
        uint cb,
        uint* lpcbNeeded,
        uint dwFilterFlag);

    /// <summary>
    ///     Retrieves the base name of the specified module.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/psapi/nf-psapi-getmodulebasenamew">GetModuleBaseNameW function (psapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process that contains the module.</param>
    /// <param name="hModule">A handle to the module.</param>
    /// <param name="lpBaseName">A pointer to the buffer that receives the base name of the module.</param>
    /// <param name="nSize">The size of the <paramref name="lpBaseName"/> buffer, in characters.</param>
    /// <returns>
    ///     The length of the string copied to the buffer, in characters, if the function succeeds;
    ///     otherwise, zero.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern uint K32GetModuleBaseNameW(
        void* hProcess,
        void* hModule,
        ushort* lpBaseName,
        uint nSize);

    /// <summary>
    ///     Retrieves the fully qualified path for the file containing the specified module.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/psapi/nf-psapi-getmodulefilenameexw">GetModuleFileNameExW function (psapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process that contains the module.</param>
    /// <param name="hModule">A handle to the module.</param>
    /// <param name="lpFilename">A pointer to a buffer that receives the fully qualified path to the module.</param>
    /// <param name="nSize">The size of the <paramref name="lpFilename"/> buffer, in characters.</param>
    /// <returns>
    ///     The length of the string copied to the buffer, in characters, if the function succeeds;
    ///     otherwise, zero.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern uint K32GetModuleFileNameExW(
        void* hProcess,
        void* hModule,
        ushort* lpFilename,
        uint nSize);

    /// <summary>
    ///     Retrieves information about the specified module in the <see cref="MODULEINFO"/> structure.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/psapi/nf-psapi-getmoduleinformation">GetModuleInformation function (psapi.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process that contains the module.</param>
    /// <param name="hModule">A handle to the module.</param>
    /// <param name="lpmodinfo">A pointer to the <see cref="MODULEINFO"/> structure that receives information about the module.</param>
    /// <param name="cb">The size of the <see cref="MODULEINFO"/> structure, in bytes.</param>
    /// <returns>
    ///     A non-zero value if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
    public static extern int K32GetModuleInformation(
        void* hProcess,
        void* hModule,
        MODULEINFO* lpmodinfo,
        uint cb);
}
