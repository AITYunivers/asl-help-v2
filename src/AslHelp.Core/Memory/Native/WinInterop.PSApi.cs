using System;
using System.Runtime.InteropServices;

using AslHelp.Common.Resources;
using AslHelp.Core.Memory.Native.Structs;

namespace AslHelp.Core.Memory.Native;

internal static unsafe partial class WinInterop
{
    /// <summary>
    ///     Retrieves a handle for each module in the specified process that meets the specified filter criteria.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/psapi/nf-psapi-enumprocessmodulesex">
    ///         EnumProcessModulesEx function (psapi.h)
    ///     </see></i>
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the process.
    /// </param>
    /// <param name="moduleHandles">
    ///     An array that receives the list of module handles.
    /// </param>
    /// <param name="cb">
    ///     The size of the <paramref name="moduleHandles"/> array, in bytes.
    /// </param>
    /// <param name="cbNeeded">
    ///     The number of bytes required to store all module handles in the lphModule array.
    /// </param>
    /// <param name="filterFlag">
    ///     The filter criteria.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the function succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool EnumProcessModules(
        nuint processHandle,
        Span<nuint> moduleHandles,
        uint cb,
        out uint cbNeeded,
        uint filterFlag)
    {
        fixed (uint* pCbNeeded = &cbNeeded)
        fixed (nuint* pModuleHandles = moduleHandles)
        {
            return EnumProcessModulesEx((void*)processHandle, (void**)pModuleHandles, cb, pCbNeeded, filterFlag) != 0;
        }

        [DllImport(Lib.Kernel32, EntryPoint = nameof(EnumProcessModulesEx), ExactSpelling = true, SetLastError = true)]
        static extern int EnumProcessModulesEx(
            void* hProcess,
            void** lphModule,
            uint cb,
            uint* lpcbNeeded,
            uint dwFilterFlag);
    }

    /// <summary>
    ///     Retrieves the base name of the specified module.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/psapi/nf-psapi-getmodulebasenamew">
    ///         GetModuleBaseNameW function (psapi.h)
    ///     </see></i>
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the process that contains the module.
    /// </param>
    /// <param name="moduleHandle">
    ///     A handle to the module.
    /// </param>
    /// <param name="baseName">
    ///     A span that receives the base name of the module.
    /// </param>
    /// <returns>
    ///     The length of the string copied to the buffer, in characters, if the function succeeds;
    ///     otherwise, zero.
    /// </returns>
    public static uint GetModuleBaseName(
        nuint processHandle,
        nuint moduleHandle,
        Span<char> baseName)
    {
        fixed (char* pBaseName = baseName)
        {
            return GetModuleBaseNameW((void*)processHandle, (void*)moduleHandle, pBaseName, (uint)baseName.Length);
        }

        [DllImport(Lib.Kernel32, EntryPoint = nameof(GetModuleBaseNameW), ExactSpelling = true, SetLastError = true)]
        static extern uint GetModuleBaseNameW(
            void* hProcess,
            void* hModule,
            char* lpBaseName,
            uint nSize);
    }

    /// <summary>
    ///     Retrieves the fully qualified path for the file containing the specified module.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/psapi/nf-psapi-getmodulefilenameexw">
    ///         GetModuleFileNameExW function (psapi.h)
    ///     </see></i>
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the process that contains the module.
    /// </param>
    /// <param name="moduleHandle">
    ///     A handle to the module.
    /// </param>
    /// <param name="fileName">
    ///     A span that receives the fully qualified path to the module.
    /// </param>
    /// <returns>
    ///     The length of the string copied to the buffer, in characters, if the function succeeds;
    ///     otherwise, zero.
    /// </returns>
    public static uint GetModuleFileName(
        nuint processHandle,
        nuint moduleHandle,
        Span<char> fileName)
    {
        fixed (char* pFileName = fileName)
        {
            return GetModuleFileNameW((void*)processHandle, (void*)moduleHandle, pFileName, (uint)fileName.Length);
        }

        [DllImport(Lib.Kernel32, EntryPoint = nameof(GetModuleFileNameW), ExactSpelling = true, SetLastError = true)]
        static extern uint GetModuleFileNameW(
            void* hProcess,
            void* hModule,
            char* lpFileName,
            uint nSize);
    }

    /// <summary>
    ///     Retrieves information about the specified module in the <see cref="MODULEINFO"/> structure.<br/>
    ///     For further information see:
    ///     <i><see href="https://docs.microsoft.com/windows/win32/api/psapi/nf-psapi-getmoduleinformation">
    ///         GetModuleInformation function (psapi.h)
    ///     </see></i>
    /// </summary>
    /// <param name="processHandle">
    ///     A handle to the process that contains the module.
    /// </param>
    /// <param name="moduleHandle">
    ///     A handle to the module.
    /// </param>
    /// <param name="moduleInfo">
    ///     A reference to the <see cref="MODULEINFO"/> structure that receives information about the module.
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the function succeeds;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool GetModuleInformation(
        nuint processHandle,
        nuint moduleHandle,
        out MODULEINFO moduleInfo)
    {
        fixed (MODULEINFO* pModuleInfo = &moduleInfo)
        {
            return GetModuleInformation((void*)processHandle, (void*)moduleHandle, pModuleInfo, (uint)sizeof(MODULEINFO)) != 0;
        }

        [DllImport(Lib.Kernel32, EntryPoint = nameof(GetModuleInformation), ExactSpelling = true, SetLastError = true)]
        static extern int GetModuleInformation(
            void* hProcess,
            void* hModule,
            MODULEINFO* lpmodinfo,
            uint cb);
    }
}
