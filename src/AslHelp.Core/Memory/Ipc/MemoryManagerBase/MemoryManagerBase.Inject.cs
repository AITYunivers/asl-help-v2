using System.IO;
using System.Linq;

using AslHelp.Core.Memory.Native;
using AslHelp.Core.Memory.Native.Enums;

namespace AslHelp.Core.Memory.Ipc;

public abstract partial class MemoryManagerBase
{
    public unsafe bool TryInjectDll(string dllPath, string? entryPoint = null)
    {
        Log($"Attempting to inject '{dllPath}'...");

        dllPath = Path.GetFullPath(dllPath);
        if (!File.Exists(dllPath))
        {
            Log($"  => Dll '{dllPath}' cannot be found!");
            return false;
        }

        uint id = (uint)Process.Id;
        nuint hProcess = WinInterop.OpenProcess(
            id,
            (uint)(ProcessAccess.CREATE_THREAD | ProcessAccess.VM_OPERATION | ProcessAccess.VM_WRITE),
            false);

        nuint hModule = WinInterop.GetModuleHandle("kernel32.dll");
        if (hModule == 0)
        {
            Log("  => Failed to get handle to kernel32.dll!");
            return false;
        }

        try
        {
            nuint pLoadLib = WinInterop.GetProcAddress(hModule, "LoadLibraryW"u8);
            if (pLoadLib == 0)
            {
                Log("  => Failed to get address of LoadLibraryW!");
                return false;
            }

            nuint length = (nuint)(dllPath.Length - 1) * sizeof(char);
            nuint pModuleAlloc = WinInterop.VirtualAlloc(hProcess, 0, length, MemState.MEM_COMMIT | MemState.MEM_RESERVE, MemProtect.PAGE_READWRITE);
            if (pModuleAlloc == 0)
            {
                Log("  => Failed to allocate memory in target process!");
                return false;
            }

            try
            {
                fixed (char* pDllPath = dllPath)
                {
                    if (!WinInteropWrapper.WriteMemory(hProcess, pModuleAlloc, pDllPath, (uint)length))
                    {
                        Log("  => Failed to write dll path to target process!");
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

        if (entryPoint is null)
        {
            Log("  => Successfully injected Dll into target process. No entry point specified.");
            return true;
        }

        Module? injectedDll = Modules.FirstOrDefault(m => m.FileName == dllPath);
        if (injectedDll is null)
        {
            Log("  => Failed to find injected dll in target process!");
            return false;
        }

        nuint dllHandle = injectedDll.Base;
        nuint pEntryPoint = WinInterop.GetProcAddress(dllHandle, entryPoint);
        if (pEntryPoint == 0)
        {
            Log($"  => Failed to get address of entry point '{entryPoint}'!");
            return false;
        }

        if (TryCreateRemoteThreadAndWaitForSuccessfulExit(hProcess, pEntryPoint, null))
        {
            Log("  => Successfully injected Dll into target process.");
            return true;
        }

        return false;
    }

    private unsafe bool TryCreateRemoteThreadAndWaitForSuccessfulExit(nuint hProcess, nuint startAddress, void* lpParameter)
    {
        nuint hThread = WinInterop.CreateRemoteThread(hProcess, null, 0, startAddress, (void*)lpParameter, 0, out _);
        if (hThread == 0)
        {
            Log("  => Failed to create remote thread in target process!");
            return false;
        }

        try
        {
            if (WinInterop.WaitForSingleObject(hThread, uint.MaxValue) != 0)
            {
                Log("  => Thread did not exit correctly!");
                return false;
            }

            if (!WinInterop.GetExitCodeThread(hThread, out uint exitCode))
            {
                Log("  => Failed to get thread exit code!");
                return false;
            }

            if (exitCode != 0)
            {
                Log($"  => Thread exited with code {exitCode}!");
                return false;
            }
        }
        finally
        {
            WinInterop.CloseHandle(hThread);
        }

        return true;
    }
}
