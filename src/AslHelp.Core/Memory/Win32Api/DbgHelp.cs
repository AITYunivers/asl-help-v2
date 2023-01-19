namespace AslHelp.Core.Memory;

#pragma warning disable IDE1006
internal static unsafe partial class Win32
{
    /// <summary>
    ///     Initializes the symbol handler for a process.<br/>
    ///     For further information, see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/dbghelp/nf-dbghelp-syminitializew">SymInitializeW function (dbghelp.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle that identifies the caller.</param>
    /// <param name="UserSearchPath">The path, or series of paths separated by a semicolon, that is used to search for symbol files.</param>
    /// <param name="fInvadeProcess">
    ///     If <see langword="true"/>, enumerates the loaded modules for the process and effectively calls the
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/dbghelp/nf-dbghelp-symloadmodule64">SymLoadModule64 function</see></i>
    ///     for each module.
    /// </param>
    /// <returns>
    ///     A non-zero value if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("dbghelp", SetLastError = true, ExactSpelling = true)]
    public static extern int SymInitializeW(
        void* hProcess,
        ushort* UserSearchPath,
        int fInvadeProcess);

    /// <summary>
    ///     Loads the symbol table for the specified module.<br/>
    ///     For further information, see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/dbghelp/nf-dbghelp-symloadmoduleexw">SymLoadModuleExW function (dbghelp.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process that was originally passed to the <see cref="SymInitializeW"/> function.</param>
    /// <param name="hFile">A handle to the file for the executable image.</param>
    /// <param name="ImageName">The name of the executable image.</param>
    /// <param name="ModuleName">A shortcut name for the module.</param>
    /// <param name="BaseOfDll">The load address of the module.</param>
    /// <param name="DllSize">The size of the module, in bytes.</param>
    /// <param name="Data">A pointer to a MODLOAD_DATA structure that represents headers other than the standard PE header.</param>
    /// <param name="Flags">If this parameter is zero, the function loads the modules and the symbols for the module.</param>
    /// <returns>
    ///     The base address of the loaded module if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("dbghelp", SetLastError = true, ExactSpelling = true)]
    public static extern ulong SymLoadModuleExW(
        void* hProcess,
        void* hFile,
        ushort* ImageName,
        ushort* ModuleName,
        ulong BaseOfDll,
        uint DllSize,
        void* Data,
        uint Flags);

    /// <summary>
    ///     Enumerates all symbols in a process.<br/>
    ///     For further information, see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/dbghelp/nf-dbghelp-symenumsymbolsw">SymEnumSymbolsW function (dbghelp.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to a process.</param>
    /// <param name="BaseOfDll">The base address of the module.</param>
    /// <param name="Mask">A wildcard string that indicates the names of the symbols to be enumerated.</param>
    /// <param name="EnumSymbolsCallback">A <see cref="PSYM_ENUMERATESYMBOLS_CALLBACK"/> callback function that receives the symbol information.</param>
    /// <param name="UserContext">A user-defined value that is passed to the callback function.</param>
    /// <returns>
    ///     A non-zero value if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("dbghelp", SetLastError = true, ExactSpelling = true)]
    public static extern int SymEnumSymbolsW(
        void* hProcess,
        ulong BaseOfDll,
        ushort* Mask,
        delegate* unmanaged[Stdcall]<SYMBOL_INFOW*, uint, void*, int> EnumSymbolsCallback,
        void* UserContext);

    /// <summary>
    ///     Deallocates all resources associated with the process handle.<br/>
    ///     For further information, see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/dbghelp/nf-dbghelp-symcleanup">SymCleanup function (dbghelp.h)</see></i>.
    /// </summary>
    /// <param name="hProcess">A handle to the process that was originally passed to the <see cref="SymInitializeW"/> function.</param>
    /// <returns>
    ///     A non-zero value if the function succeeds;
    ///     otherwise, 0.
    /// </returns>
    [DllImport("dbghelp", SetLastError = true, ExactSpelling = true)]
    public static extern int SymCleanup(
        void* hProcess);
}
#pragma warning restore IDE1006
