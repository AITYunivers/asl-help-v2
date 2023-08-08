using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;

using AslHelp.Core.Memory.Native.Enums;
using AslHelp.Core.Memory.Native.Structs;

namespace AslHelp.Core.Memory.Native;

internal static unsafe partial class WinInteropWrapper
{
    public static bool IsInjected(nuint processHandle, uint processId, string dllPath, [NotNullWhen(true)] out Module? module)
    {
        dllPath = Path.GetFullPath(dllPath);

        foreach (MODULEENTRY32W me in EnumerateModulesTh32(processId))
        {
            string fileName = new((char*)me.szExePath);
            if (fileName.Equals(dllPath, StringComparison.OrdinalIgnoreCase))
            {
                string name = new((char*)me.szModule);
                nuint @base = (nuint)me.modBaseAddr;
                uint memorySize = me.modBaseSize;

                module = new(processHandle, name, fileName, @base, memorySize);
                return true;
            }
        }

        module = null;
        return false;
    }

    public static unsafe bool TryInjectDll(nuint processHandle, string dllPath)
    {
        dllPath = Path.GetFullPath(dllPath);
        if (!File.Exists(dllPath))
        {
            Debug.Warn("    => Dll file cannot be found.");
            return false;
        }

        nuint hModule = WinInterop.GetModuleHandle("kernel32.dll");
        if (hModule == 0)
        {
            Debug.Warn("    => Failed to get kernel32.dll handle.");
            return false;
        }

        try
        {
            nuint pLoadLib = WinInterop.GetProcAddress(hModule, "LoadLibraryW"u8);
            if (pLoadLib == 0)
            {
                Debug.Warn("    => Failed to get LoadLibraryW address.");
                return false;
            }

            nuint length = (nuint)(dllPath.Length + 1) * sizeof(char);
            nuint pModuleAlloc = WinInterop.VirtualAlloc(processHandle, 0, length, MemState.MEM_COMMIT | MemState.MEM_RESERVE, MemProtect.PAGE_READWRITE);
            if (pModuleAlloc == 0)
            {
                Debug.Warn("    => Failed to allocate memory in target process.");
                return false;
            }

            try
            {
                fixed (char* pDllPath = dllPath)
                {
                    if (!WriteMemory(processHandle, pModuleAlloc, pDllPath, (uint)length))
                    {
                        Debug.Warn("    => Failed to write dll path to target process.");
                        return false;
                    }
                }

                if (!TryCreateRemoteThreadAndWaitForExit(processHandle, pLoadLib, (void*)pModuleAlloc, out uint exitCode))
                {
                    return false;
                }

                if (exitCode <= 0)
                {
                    int err = Marshal.GetLastWin32Error();
                    Debug.Warn($"    => ExitCode was not a valid handle (0x{exitCode:X}).");
                    Debug.Warn($"       0x{err:X}: '{new Win32Exception(err).Message}'");

                    return false;
                }
            }
            finally
            {
                WinInterop.VirtualFree(processHandle, pModuleAlloc, 0, MemState.MEM_RELEASE);
            }
        }
        finally
        {
            WinInterop.CloseHandle(hModule);
        }

        return true;
    }

    private static unsafe bool TryCreateRemoteThreadAndWaitForExit(nuint hProcess, nuint startAddress, void* lpParameter, out uint exitCode)
    {
        nuint hThread = WinInterop.CreateRemoteThread(hProcess, null, 0, startAddress, lpParameter, 0, out _);
        if (hThread == 0)
        {
            Debug.Warn("    => Failed to create remote thread.");

            exitCode = uint.MaxValue;
            return false;
        }

        try
        {
            if (WinInterop.WaitForSingleObject(hThread, uint.MaxValue) != 0)
            {
                Debug.Warn("    => Failed to wait for remote thread.");

                exitCode = uint.MaxValue;
                return false;
            }

            if (!WinInterop.GetExitCodeThread(hThread, out exitCode))
            {
                Debug.Warn("    => Failed to get remote thread exit code.");
                return false;
            }

            return true;
        }
        finally
        {
            WinInterop.CloseHandle(hThread);
        }
    }

    public static bool TryCallEntryPoint(nuint hProcess, nuint hModule, ReadOnlySpan<byte> entryPoint)
    {
        nuint pEntryPoint = WinInterop.GetProcAddress(hModule, entryPoint);
        if (pEntryPoint == 0)
        {
            int err = Marshal.GetLastWin32Error();
            Debug.Warn($"    => Entry point not found.");
            Debug.Warn($"       0x{err:X}: '{new Win32Exception(err).Message}'");

            return false;
        }

        return TryCreateRemoteThreadAndWaitForExit(hProcess, pEntryPoint, null, out uint exitCode)
            && exitCode == 0;
    }
}
