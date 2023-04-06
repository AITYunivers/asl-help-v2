using AslHelp.Core.Exceptions;

namespace AslHelp.Core.Memory.Pipes;

public static unsafe class Injector
{
    public static bool TryInject(this Process process, string modulePath)
    {
        modulePath = Path.GetFullPath(modulePath);
        if (!File.Exists(modulePath))
        {
            ThrowHelper.Throw.FileNotFound(modulePath, "Unable to find the specified module to inject.");
        }

        nint processHandle = process.Handle;
        int processId = process.Id;

        foreach (Module module in Native.ModulesTh32(processHandle, processId))
        {
            if (module.FilePath == modulePath)
            {
                return true;
            }
        }

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
}
