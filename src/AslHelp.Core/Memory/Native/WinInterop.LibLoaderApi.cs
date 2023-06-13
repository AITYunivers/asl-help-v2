using System.Runtime.InteropServices;

using AslHelp.Common.Resources;

namespace AslHelp.Core.Memory.Native;

internal static unsafe partial class WinInterop
{
    /// <summary>
    ///     Retrieves a module handle for the specified module. The module must have been loaded by the calling process.<br/>
    ///     For further information, see:
    ///     <i><see href="https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getmodulehandlew">
    ///         GetModuleHandleW function (libloaderapi.h)
    ///     </see></i>
    /// </summary>
    /// <param name="moduleName">
    ///     The name of the loaded module.
    /// </param>
    /// <returns>
    ///     A handle to the specified module, if the function succeeds;
    ///     otherwise, <see langword="null"/>.
    /// </returns>
    public static nuint GetModuleHandle(string? moduleName)
    {
        fixed (char* pModuleName = moduleName)
        {
            return (nuint)GetModuleHandleW((ushort*)pModuleName);
        }

        [DllImport(Lib.Kernel32, EntryPoint = nameof(GetModuleHandleW), ExactSpelling = true, SetLastError = true)]
        static extern void* GetModuleHandleW(
            ushort* lpModuleName);
    }

    /// <summary>
    ///     Retrieves the address of an exported function or variable from the specified DLL.<br/>
    ///     For further information, see:
    ///     <i><see href="https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-getprocaddress">
    ///         GetProcAddress function (libloaderapi.h)
    ///     </see></i>
    /// </summary>
    /// <param name="moduleHandle">
    ///     A handle to the DLL module that contains the function or variable.
    /// </param>
    /// <param name="procName">
    ///     The function or variable name, or the function's ordinal value.
    /// </param>
    /// <returns>
    ///     The address of the exported function or variable, if the function succeeds;
    ///     otherwise, <see langword="null"/>.
    /// </returns>
    public static nuint GetProcAddress(nuint moduleHandle, string? procName)
    {
        fixed (char* pProcName = procName)
        {
            return (nuint)GetProcAddress((void*)moduleHandle, (byte*)pProcName);
        }

        [DllImport(Lib.Kernel32, EntryPoint = nameof(GetProcAddress), ExactSpelling = true, SetLastError = true)]
        static extern void* GetProcAddress(
            void* hModule,
            byte* lpProcName);
    }
}