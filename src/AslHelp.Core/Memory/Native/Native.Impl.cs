using System.ComponentModel;
using AslHelp.Core.Exceptions;
using AslHelp.Core.Memory.Models;

namespace AslHelp.Core.Memory;

internal static unsafe partial class Native
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Is64Bit(this Process process)
    {
        ThrowHelper.ThrowIfNullOrExited(process);

        int wow64;
        if (IsWow64Process(process.Handle, &wow64) == 0)
        {
            throw new Win32Exception();
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
            throw new Win32Exception();
        }

        int numModules = (int)(cbNeeded / Marshal.SizeOf<nint>());
        nint[] hModule = new nint[numModules];

        if (!EnumProcessModulesEx(hProcess, hModule, cbNeeded, out _))
        {
            throw new Win32Exception();
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

            yield return new(baseName, fileName, moduleInfo);
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
                throw new Win32Exception();
            }

            do
            {
                yield return new(me);
            } while (Module32NextW(snapshot, ref me));
        }
        finally
        {
            CloseHandle(snapshot);
        }
    }

    public static IEnumerable<MemoryPage> MemoryPages(this Process process, bool is64Bit)
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

            yield return new(mbi);

            if (addr >= max)
            {
                break;
            }
        }
    }

    public static List<DebugSymbol> Symbols(this Module module, Process process)
    {
        ThrowHelper.ThrowIfNullOrExited(process);
        ThrowHelper.ThrowIfNull(module);

        nint hProcess = process.Handle;

        List<DebugSymbol> syms = new();
        void* pSyms = Unsafe.AsPointer(ref syms);

        getSymbols(null);
        getSymbols(Path.GetDirectoryName(module.FilePath));

        return syms;

        void getSymbols(string pdbDirectory)
        {
            if (!SymInitializeW(hProcess, pdbDirectory))
            {
                throw new Win32Exception();
            }

            try
            {
                if (!SymLoadModuleExW(hProcess, module))
                {
                    throw new Win32Exception();
                }

                if (!SymEnumSymbolsW(hProcess, module, pSyms))
                {
                    throw new Win32Exception();
                }
            }
            finally
            {
                SymCleanup((void*)hProcess);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static nint AllocateRemoteString(this Process process, string value)
    {
        uint length = sizeof(char) * ((uint)value.Length + 1);
        void* memory = VirtualAllocEx(
            (void*)process.Handle,
            null,
            length,
            MemState.MEM_COMMIT | MemState.MEM_RESERVE,
            MemProtect.PAGE_READWRITE);

        fixed (char* pValue = value)
        {
            nuint nWritten;

            if (WriteProcessMemory((void*)process.Handle, memory, pValue, length, &nWritten) == 0
                || nWritten != length)
            {
                throw new Win32Exception();
            }
        }

        return (nint)memory;
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
