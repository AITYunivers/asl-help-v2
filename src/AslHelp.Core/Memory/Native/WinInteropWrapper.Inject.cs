using System;
using System.Diagnostics;
using System.IO;

using AslHelp.Core.Memory.Native.Enums;
using AslHelp.Core.Memory.Native.Structs;

namespace AslHelp.Core.Memory.Native;

internal static unsafe partial class WinInteropWrapper
{
    public static bool IsInjected(this Process process, string dllPath, out nuint module)
    {
        dllPath = Path.GetFullPath(dllPath);

        foreach (MODULEENTRY32W me in process.EnumerateModulesTh32())
        {
            string fileName = new((char*)me.szExePath);
            if (fileName.Equals(dllPath, StringComparison.OrdinalIgnoreCase))
            {
                module = (nuint)me.modBaseAddr;
                return true;
            }
        }

        module = default;
        return false;
    }

    public static unsafe bool TryInjectDll(this Process process, string dllPath, string? entryPoint)
    {
        dllPath = Path.GetFullPath(dllPath);
        if (!File.Exists(dllPath))
        {
            return false;
        }

        uint id = (uint)process.Id;
        nuint hProcess = WinInterop.OpenProcess(
            id,
            (uint)(ProcessAccess.CREATE_THREAD | ProcessAccess.VM_OPERATION | ProcessAccess.VM_WRITE),
            false);

        nuint hModule = WinInterop.GetModuleHandle("kernel32.dll");
        if (hModule == 0)
        {
            return false;
        }

        try
        {
            nuint pLoadLib = WinInterop.GetProcAddress(hModule, "LoadLibraryW"u8);
            if (pLoadLib == 0)
            {
                return false;
            }

            nuint length = (nuint)(dllPath.Length - 1) * sizeof(char);
            nuint pModuleAlloc = WinInterop.VirtualAlloc(hProcess, 0, length, MemState.MEM_COMMIT | MemState.MEM_RESERVE, MemProtect.PAGE_READWRITE);
            if (pModuleAlloc == 0)
            {
                return false;
            }

            try
            {
                fixed (char* pDllPath = dllPath)
                {
                    if (!WriteMemory(hProcess, pModuleAlloc, pDllPath, (uint)length))
                    {
                        return false;
                    }
                }

                if (!TryCreateRemoteThreadAndWaitForSuccessfulExit(hProcess, pLoadLib, (void*)pModuleAlloc))
                {
                    return false;
                }
            }
            finally
            {
                WinInterop.VirtualFree(hProcess, pModuleAlloc, 0, MemState.MEM_RELEASE);
            }
        }
        finally
        {
            WinInterop.CloseHandle(hModule);
        }

        return true;
    }

    private static unsafe bool TryCreateRemoteThreadAndWaitForSuccessfulExit(nuint hProcess, nuint startAddress, void* lpParameter)
    {
        nuint hThread = WinInterop.CreateRemoteThread(hProcess, null, 0, startAddress, lpParameter, 0, out _);
        if (hThread == 0)
        {
            return false;
        }

        try
        {
            return WinInterop.WaitForSingleObject(hThread, uint.MaxValue) == 0
                && WinInterop.GetExitCodeThread(hThread, out uint exitCode)
                && exitCode == 0;
        }
        finally
        {
            WinInterop.CloseHandle(hThread);
        }
    }

    public static bool TryCallEntryPoint(nuint hProcess, nuint hModule, string entryPoint)
    {
        nuint pEntryPoint = WinInterop.GetProcAddress(hModule, entryPoint);
        if (pEntryPoint == 0)
        {
            return false;
        }

        return TryCreateRemoteThreadAndWaitForSuccessfulExit(hProcess, pEntryPoint, null);
    }
}
