using AslHelp.Core.Exceptions;
using AslHelp.Core.Memory.Models;
using static AslHelp.Core.Memory.WinApi;

namespace AslHelp.Core.Memory;

public static unsafe class Native
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Is64Bit(this Process process)
    {
        ThrowHelper.ThrowIfNullOrExited(process);

        int wow64;
        if (IsWow64Process((void*)process.Handle, &wow64) == 0)
        {
            ThrowHelper.Throw.Win32();
        }

        return Environment.Is64BitOperatingSystem && wow64 == 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Read(this Process process, nint address, void* buffer, int bufferSize)
    {
        ThrowHelper.ThrowIfNullOrExited(process);

        nuint nSize = (nuint)bufferSize, nRead;

        return ReadProcessMemory((void*)process.Handle, (void*)address, buffer, nSize, &nRead) != 0
            && nRead == nSize;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Write(this Process process, nint address, void* data, int dataSize)
    {
        ThrowHelper.ThrowIfNullOrExited(process);

        nuint nSize = (nuint)dataSize, nWritten;

        return WriteProcessMemory((void*)process.Handle, (void*)address, data, nSize, &nWritten) != 0
            && nWritten == nSize;
    }

    public static IEnumerable<Module> Modules(this Process process)
    {
        ThrowHelper.ThrowIfNullOrExited(process);

        nint hProcess = process.Handle;

        if (EnumProcessModulesEx(hProcess, null, 0, out uint cbNeeded))
        {
            ThrowHelper.Throw.Win32();
        }

        int numModules = (int)(cbNeeded / Marshal.SizeOf<nint>());
        nint[] hModule = new nint[numModules];

        if (!EnumProcessModulesEx(hProcess, hModule, cbNeeded, out _))
        {
            ThrowHelper.Throw.Win32();
        }

        for (int i = 0; i < numModules; i++)
        {
            if (!GetModuleBaseNameW(hProcess, hModule[i], out string baseName))
            {
                yield break;
            }

            if (!GetModuleFileNameExW(hProcess, hModule[i], out string fileName))
            {
                yield break;
            }

            if (!GetModuleInformation(hProcess, hModule[i], out MODULEINFO moduleInfo))
            {
                yield break;
            }

            yield return new(process, baseName, fileName, moduleInfo);
        }
    }

    public static IEnumerable<Module> ModulesTh32(this Process process)
    {
        MODULEENTRY32W me = new() { dwSize = MODULEENTRY32W.Size };
        nint snapshot = CreateToolhelp32Snapshot(ThFlags.TH32CS_SNAPMODULE | ThFlags.TH32CS_SNAPMODULE32, process.Id);

        try
        {
            if (!Module32FirstW(snapshot, ref me))
            {
                ThrowHelper.Throw.Win32();
            }

            do
            {
                yield return new(process, me);
            } while (Module32NextW(snapshot, ref me));
        }
        finally
        {
            _ = CloseHandle(snapshot);
        }
    }

    public static IEnumerable<MemoryPage> MemoryPages(this Process process, bool is64Bit, bool allPages)
    {
        ThrowHelper.ThrowIfNullOrExited(process);

        nint addr = 0x10000, max = (nint)(is64Bit ? 0x7FFFFFFEFFFF : 0x7FFEFFFF);

        while (VirtualQueryEx(process.Handle, addr, out MEMORY_BASIC_INFORMATION mbi))
        {
            addr += (nint)mbi.RegionSize;

            if (mbi.State != MemState.MEM_COMMIT)
            {
                continue;
            }

            if (!allPages && (mbi.Protect & MemProtect.PAGE_GUARD) != 0)
            {
                continue;
            }

            if (!allPages && mbi.Type != MemType.MEM_PRIVATE)
            {
                continue;
            }

            yield return new(mbi);

            if (addr >= max)
            {
                break;
            }
        }
    }

    public static Dictionary<string, DebugSymbol> Symbols(this Module module, Process process)
    {
        ThrowHelper.ThrowIfNullOrExited(process);
        ThrowHelper.ThrowIfNull(module);

        nint hProcess = process.Handle;

        Dictionary<string, DebugSymbol> syms = new(StringComparer.OrdinalIgnoreCase);
        void* pSyms = Unsafe.AsPointer(ref syms);

        getSymbols(null);
        getSymbols(Path.GetDirectoryName(module.FilePath));

        return syms;

        void getSymbols(string pdbDirectory)
        {
            if (!SymInitializeW(hProcess, pdbDirectory))
            {
                ThrowHelper.Throw.Win32();
            }

            try
            {
                if (!SymLoadModuleExW(hProcess, module))
                {
                    ThrowHelper.Throw.Win32();
                }

                if (!SymEnumSymbolsW(hProcess, module, pSyms))
                {
                    ThrowHelper.Throw.Win32();
                }
            }
            finally
            {
                _ = SymCleanup((void*)hProcess);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nint AllocateRemoteString(this Process process, string value)
    {
        ThrowHelper.ThrowIfNullOrExited(process);

        return (nint)AllocateRemoteString((void*)process.Handle, value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void* AllocateRemoteString(void* hProcess, string value)
    {
        if (hProcess == null)
        {
            ThrowHelper.Throw.ArgumentNull(nameof(hProcess));
        }

        uint length = (uint)((value.Length + 1) * sizeof(char));
        void* memory = VirtualAllocEx(
            hProcess,
            null,
            length,
            MemState.MEM_COMMIT | MemState.MEM_RESERVE,
            MemProtect.PAGE_READWRITE);

        if (memory == null)
        {
            ThrowHelper.Throw.Win32();
        }

        fixed (char* pValue = value)
        {
            nuint nWritten;

            if (WriteProcessMemory(hProcess, memory, pValue, length, &nWritten) == 0
                || nWritten != length)
            {
                ThrowHelper.Throw.Win32();
            }
        }

        return memory;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPointer<T>()
    {
        return typeof(T) == typeof(nint) || typeof(T) == typeof(nuint) || typeof(T) == typeof(IntPtr) || typeof(T) == typeof(UIntPtr);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetTypeSize<T>(bool is64Bit) where T : unmanaged
    {
        return IsPointer<T>() ? (is64Bit ? 0x8 : 0x4) : sizeof(T);
    }
}
