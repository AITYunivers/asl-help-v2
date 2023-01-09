using System;
using System.Runtime.InteropServices;
using System.Text;

public static class Injector
{
    public static bool Inject(int pid, string dllPath)
    {
        // create dll path buffer
        int len = (dllPath.Length + 1) * 2;    // + 1 for null termination
        var bytes = new byte[len];
        Encoding.Unicode.GetBytes(dllPath, 0, dllPath.Length, bytes, 0);

        // get handle to remote process
        IntPtr hProcess = OpenProcess(
            ProcessAccessFlags.CreateThread |
            ProcessAccessFlags.VirtualMemoryOperation |
            ProcessAccessFlags.VirtualMemoryWrite,
            false,
            pid);

        // write dll path into process
        IntPtr pRemoteLibPath = VirtualAllocEx(
            hProcess, IntPtr.Zero, (uint)len, AllocationType.Commit, MemoryProtection.ReadWrite);
        WriteProcessMemory(hProcess, pRemoteLibPath, bytes, len, out var _);

        // get ptr to LoadLibrary function
        IntPtr pModule = GetModuleHandle("kernel32.dll");
        IntPtr pLoadLibrary = GetProcAddress(pModule, "LoadLibraryW");

        // create remote thread that calls LoadLibrary
        IntPtr pThread = CreateRemoteThread(hProcess,
            IntPtr.Zero, 0, pLoadLibrary, pRemoteLibPath, 0, out var _);

        // wait for LoadLibrary to be done and cleanup
        WaitForSingleObject(pThread, uint.MaxValue);

        GetExitCodeThread(pThread, out uint code);

        CloseHandle(pThread);
        VirtualFreeEx(hProcess, pRemoteLibPath, 0, AllocationType.Release);
        CloseHandle(hProcess);

        return code != 0;
    }

    #region NativeMethods

    [Flags]
    enum ProcessAccessFlags : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VirtualMemoryOperation = 0x00000008,
        VirtualMemoryRead = 0x00000010,
        VirtualMemoryWrite = 0x00000020,
        DuplicateHandle = 0x00000040,
        CreateProcess = 0x000000080,
        SetQuota = 0x00000100,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        QueryLimitedInformation = 0x00001000,
        Synchronize = 0x00100000
    }

    [Flags]
    enum AllocationType : uint
    {
        Commit = 0x1000,
        Reserve = 0x2000,
        Decommit = 0x4000,
        Release = 0x8000,
        Reset = 0x80000,
        Physical = 0x400000,
        TopDown = 0x100000,
        WriteWatch = 0x200000,
        LargePages = 0x20000000
    }

    [Flags]
    enum MemoryProtection : uint
    {
        Execute = 0x10,
        ExecuteRead = 0x20,
        ExecuteReadWrite = 0x40,
        ExecuteWriteCopy = 0x80,
        NoAccess = 0x01,
        ReadOnly = 0x02,
        ReadWrite = 0x04,
        WriteCopy = 0x08,
        GuardModifierflag = 0x100,
        NoCacheModifierflag = 0x200,
        WriteCombineModifierflag = 0x400
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr OpenProcess(
        ProcessAccessFlags processAccess,
        bool bInheritHandle,
        int processId
    );

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
        uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

    [DllImport("kernel32.dll")]
    private static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        int nSize,
        out IntPtr lpNumberOfBytesWritten
    );

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll")]
    private static extern IntPtr CreateRemoteThread(IntPtr hProcess,
       IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress,
       IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
        int dwSize, AllocationType dwFreeType);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);

    #endregion
}
