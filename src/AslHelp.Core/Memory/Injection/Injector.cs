using AslHelp.Core.Exceptions;
using AslHelp.Core.IO;

namespace AslHelp.Core.Memory.Pipes;

public static unsafe class Injector
{
    public static bool TryInjectAslCoreNative(this Process process)
    {
        if (process.Is64Bit())
        {
            return process.TryInjectDllFromEmbeddedResource(AHR.NativeLib64, "x64");
        }
        else
        {
            return process.TryInjectDllFromEmbeddedResource(AHR.NativeLib86, "x86");
        }
    }

    public static bool TryInjectDllFromEmbeddedResource(this Process process, string resource, string unpackDirectory)
    {
        string targetFile = Path.GetFullPath($"{unpackDirectory}/{resource}");

        if (process.DllIsInjected(targetFile))
        {
            return true;
        }

        try
        {
            ResourceManager.UnpackResource(resource, targetFile);
        }
        catch (IOException ex) when ((uint)ex.HResult == 0x80070020) { } // File being used by another process.

        return process.TryInjectDll(targetFile);
    }

    public static bool TryInjectDll(this Process process, string modulePath)
    {
        modulePath = Path.GetFullPath(modulePath);
        if (!File.Exists(modulePath))
        {
            ThrowHelper.Throw.FileNotFound(modulePath, "Unable to find the specified module to inject.");
        }

        int processId = process.Id;

        void* hProcess = WinApi.OpenProcess(processId, (uint)(ProcessAccess.CREATE_THREAD | ProcessAccess.VM_OPERATION | ProcessAccess.VM_WRITE));
        if (hProcess == null)
        {
            return false;
        }

        try
        {
            void* hModule;
            fixed (char* pKernel32 = "kernel32.dll")
            {
                hModule = WinApi.GetModuleHandleW((ushort*)pKernel32);
                if (hModule == null)
                {
                    return false;
                }
            }

            void* pLoadLib;
            fixed (byte* pLoadLibraryW = "LoadLibraryW"u8)
            {
                pLoadLib = WinApi.GetProcAddress(hModule, (sbyte*)pLoadLibraryW);
                if (pLoadLib == null)
                {
                    return false;
                }
            }

            void* nullPtr = (void*)0;
            int length = (modulePath.Length + 1) * sizeof(char);

            void* pModuleAlloc = WinApi.VirtualAllocEx(hProcess, nullPtr, (nuint)length, MemState.MEM_COMMIT | MemState.MEM_RESERVE, MemProtect.PAGE_READWRITE);
            if (pModuleAlloc == null)
            {
                return false;
            }

            fixed (char* pModulePath = modulePath)
            {
                nuint bytesWritten;
                if (WinApi.WriteProcessMemory(hProcess, pModuleAlloc, pModulePath, (nuint)length, &bytesWritten) == 0
                    || bytesWritten != (nuint)length)
                {
                    return false;
                }
            }

            void* hThread = WinApi.CreateRemoteThread(hProcess, nullPtr, 0, pLoadLib, pModuleAlloc, 0, null);
            if (hThread == null)
            {
                return false;
            }

            uint exitCode;
            try
            {
                if (WinApi.WaitForSingleObject(hThread, uint.MaxValue) != 0)
                {
                    ThrowHelper.Throw.Win32();
                }

                if (WinApi.GetExitCodeThread(hThread, &exitCode) == 0)
                {
                    ThrowHelper.Throw.Win32();
                }
            }
            finally
            {
                _ = WinApi.CloseHandle(hThread);
            }

            if (WinApi.VirtualFreeEx(hProcess, pModuleAlloc, 0, MemState.MEM_RELEASE) == 0)
            {
                ThrowHelper.Throw.Win32();
            }

            return exitCode != 0;
        }
        finally
        {
            _ = WinApi.CloseHandle(hProcess);
        }
    }

    public static bool DllIsInjected(this Process process, string modulePath)
    {
        foreach (Module module in Native.ModulesTh32(process.Handle, process.Id))
        {
            if (module.FilePath == modulePath)
            {
                return true;
            }
        }

        return false;
    }
}
