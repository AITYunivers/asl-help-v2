using System.Runtime.InteropServices;

using AslHelp.Common.Memory;
using AslHelp.Core.Memory.Native.Structs;

namespace AslHelp.Core.Memory.Native;

#pragma warning disable IDE1006
internal static unsafe partial class WinInterop
{
    /// <summary>
    ///     Initializes the symbol handler for a process.<br/>
    ///     For further information, see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/dbghelp/nf-dbghelp-syminitializew">
    ///         SymInitializeW function (dbghelp.h)
    ///     </see></i>
    /// </summary>
    /// <param name="processHandle">
    ///     A handle that identifies the caller.
    /// </param>
    /// <param name="userSearchPath">
    ///     The path, or series of paths separated by a semicolon, that is used to search for symbol files.
    /// </param>
    /// <param name="invadeProcess">
    ///     If <see langword="true"/>, enumerates the loaded modules for the process and effectively calls the
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/dbghelp/nf-dbghelp-symloadmodule64">SymLoadModule64 function</see></i>
    ///     for each module.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the function succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool SymInitialize(nuint processHandle, string? userSearchPath, bool invadeProcess)
    {
        fixed (char* pUserSearchPath = userSearchPath)
        {
            return SymInitializeW((void*)processHandle, (ushort*)pUserSearchPath, invadeProcess ? 1 : 0) != 0;
        }

        [DllImport(Lib.DbgHelp, EntryPoint = nameof(SymInitializeW), ExactSpelling = true, SetLastError = true)]
        static extern int SymInitializeW(
            void* hProcess,
            ushort* UserSearchPath,
            int fInvadeProcess);
    }

    /// <summary>
    ///     Loads the symbol table for the specified module.<br/>
    ///     For further information, see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/dbghelp/nf-dbghelp-symloadmoduleexw">
    ///         SymLoadModuleExW function (dbghelp.h)
    ///     </see></i>
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the process that was originally passed to the <see cref="SymInitialize"/> function.
    /// </param>
    /// <param name="fileHandle">
    ///     A handle to the file for the executable image.
    /// </param>
    /// <param name="imageName">
    ///     The name of the executable image.
    /// </param>
    /// <param name="moduleName">
    ///     A shortcut name for the module.
    /// </param>
    /// <param name="moduleBase">
    ///     The load address of the module.
    /// </param>
    /// <param name="memorySize">
    ///     The size of the module, in bytes.
    /// </param>
    /// <param name="data">
    ///     A pointer to a MODLOAD_DATA structure that represents headers other than the standard PE header.
    /// </param>
    /// <param name="flags">
    ///     If this parameter is zero, the function loads the modules and the symbols for the module.
    /// </param>
    /// <returns>
    ///     The base address of the loaded module if the function succeeds;
    ///     otherwise, <see langword="null"/>.
    /// </returns>
    public static nuint SymLoadModule(
        nuint processHandle,
        nuint fileHandle,
        string imageName,
        string? moduleName,
        nuint moduleBase,
        uint memorySize,
        void* data,
        uint flags)
    {
        fixed (char* pImageName = imageName, pModuleName = moduleName)
        {
            return (nuint)SymLoadModuleExW(
                (void*)processHandle,
                (void*)fileHandle,
                (ushort*)pImageName,
                (ushort*)pModuleName,
                moduleBase,
                memorySize,
                data,
                flags);
        }

        [DllImport(Lib.DbgHelp, EntryPoint = nameof(SymLoadModuleExW), ExactSpelling = true, SetLastError = true)]
        static extern ulong SymLoadModuleExW(
            void* hProcess,
            void* hFile,
            ushort* ImageName,
            ushort* ModuleName,
            ulong BaseOfDll,
            uint DllSize,
            void* Data,
            uint Flags);
    }

    /// <summary>
    ///     Enumerates all symbols in a process.<br/>
    ///     For further information, see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/dbghelp/nf-dbghelp-symenumsymbolsw">
    ///         SymEnumSymbolsW function (dbghelp.h)
    ///     </see></i>
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the process that was originally passed to the <see cref="SymInitialize"/> function.
    /// </param>
    /// <param name="moduleBase">
    ///     The base address of the module.
    /// </param>
    /// <param name="mask">
    ///     A wildcard string that indicates the names of the symbols to be enumerated.
    /// </param>
    /// <param name="pCallback">
    /// A PSYM_ENUMERATESYMBOLS_CALLBACK callback function that receives the symbol information.
    /// </param>
    /// <param name="symbols">
    ///     A list of <see cref="DebugSymbol"/>s that will be filled with the enumerated symbols.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the function succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool SymEnumSymbols(
        nuint processHandle,
        nuint moduleBase,
        string? mask,
        delegate* unmanaged[Stdcall]<SYMBOL_INFOW*, uint, void*, int> pCallback,
        void* context)
    {
        fixed (char* pMask = mask)
        {
            return SymEnumSymbolsW(
                (void*)processHandle,
                moduleBase,
                (ushort*)pMask,
                pCallback,
                context) != 0;
        }

        [DllImport(Lib.DbgHelp, EntryPoint = nameof(SymEnumSymbolsW), ExactSpelling = true, SetLastError = true)]
        static extern int SymEnumSymbolsW(
            void* hProcess,
            ulong BaseOfDll,
            ushort* Mask,
            delegate* unmanaged[Stdcall]<SYMBOL_INFOW*, uint, void*, int> EnumSymbolsCallback,
            void* UserContext);
    }

    /// <summary>
    ///     Deallocates all resources associated with the process handle.<br/>
    ///     For further information, see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/dbghelp/nf-dbghelp-symcleanup">
    ///         SymCleanup function (dbghelp.h)
    ///     </see></i>
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the process that was originally passed to the <see cref="SymInitialize"/> function.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the function succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool SymCleanup(nuint processHandle)
    {
        return SymCleanup((void*)processHandle) != 0;

        [DllImport(Lib.DbgHelp, EntryPoint = nameof(SymCleanup), ExactSpelling = true, SetLastError = true)]
        static extern int SymCleanup(
            void* hProcess);
    }
}
#pragma warning restore IDE1006
