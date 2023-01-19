using AslHelp.Core.Exceptions;

namespace AslHelp.Core.Memory.Injection;

public static unsafe class Injector
{
    public static bool TryInject(this Process process, string modulePath)
    {
        foreach (Module module in process.ModulesTh32())
        {
            if (module.FilePath == modulePath)
            {
                return true;
            }
        }

        modulePath = Path.GetFullPath(modulePath);
        if (!File.Exists(modulePath))
        {
            ThrowHelper.Throw.FileNotFound(modulePath, "Unable to find the specified module to inject.");
        }

        void* hProcess = Win32.OpenProcess(process.Id, (uint)(ProcessAccess.CREATE_THREAD | ProcessAccess.VM_OPERATION | ProcessAccess.VM_WRITE));
        if (hProcess == null)
        {
            return false;
        }

        void* nullPtr = (void*)0;
        int length = (modulePath.Length + 1) * sizeof(char);

        try
        {
            void* hModule;
            fixed (char* pKernel32 = "kernel32.dll")
            {
                hModule = Win32.GetModuleHandleW((ushort*)pKernel32);
                if (hModule == null)
                {
                    return false;
                }
            }

            void* pLoadLib;
            fixed (byte* pLoadLibraryW = "LoadLibraryW"u8)
            {
                pLoadLib = Win32.GetProcAddress(hModule, (sbyte*)pLoadLibraryW);
                if (pLoadLib == null)
                {
                    return false;
                }
            }

            void* pModuleAlloc = Win32.VirtualAllocEx(hProcess, nullPtr, (nuint)length, MemState.MEM_COMMIT | MemState.MEM_RESERVE, MemProtect.PAGE_READWRITE);
            if (pModuleAlloc == null)
            {
                return false;
            }

            fixed (char* pModulePath = modulePath)
            {
                nuint bytesWritten;
                if (Win32.WriteProcessMemory(hProcess, pModuleAlloc, pModulePath, (nuint)length, &bytesWritten) == 0
                    || bytesWritten != (nuint)length)
                {
                    return false;
                }
            }

            void* hThread = Win32.CreateRemoteThread(hProcess, nullPtr, 0, pLoadLib, pModuleAlloc, 0, null);
            if (hThread == null)
            {
                return false;
            }

            uint exitCode;
            try
            {
                if (Win32.WaitForSingleObject(hThread, uint.MaxValue) == 0)
                {
                    ThrowHelper.Throw.Win32();
                }

                if (Win32.GetExitCodeThread(hThread, &exitCode) == 0)
                {
                    ThrowHelper.Throw.Win32();
                }
            }
            finally
            {
                Win32.CloseHandle(hThread);
            }

            if (Win32.VirtualFreeEx(hProcess, pModuleAlloc, 0, MemState.MEM_RELEASE) == 0)
            {
                ThrowHelper.Throw.Win32();
            }

            return exitCode != 0;
        }
        finally
        {
            Win32.CloseHandle(hProcess);
        }
    }
}
