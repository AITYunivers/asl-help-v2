using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;
using AslHelp.Core.Memory.Models;
using static AslHelp.Core.Memory.WinApi;

namespace AslHelp.Core.Memory;

public static unsafe class Native
{
    public static bool Is64Bit(this Process process)
    {
        return Is64Bit(process.Handle);
    }

    public static bool Is64Bit(nint processHandle)
    {
        int wow64;
        if (IsWow64Process((void*)processHandle, &wow64) == 0)
        {
            ThrowHelper.ThrowWin32Exception();
        }

        return Environment.Is64BitOperatingSystem && wow64 == 0;
    }

    public static bool Read(this Process process, nint address, void* buffer, int bufferSize)
    {
        return Read(process.Handle, address, buffer, bufferSize);
    }

    public static bool Read(nint processHandle, nint address, void* buffer, int bufferSize)
    {
        nuint nSize = (nuint)bufferSize, nRead;

        return ReadProcessMemory((void*)processHandle, (void*)address, buffer, nSize, &nRead) != 0
            && nRead == nSize;
    }

    public static bool Write(this Process process, nint address, void* data, int dataSize)
    {
        return Read(process.Handle, address, data, dataSize);
    }

    public static bool Write(nint processHandle, nint address, void* data, int dataSize)
    {
        nuint nSize = (nuint)dataSize, nWritten;

        return WriteProcessMemory((void*)processHandle, (void*)address, data, nSize, &nWritten) != 0
            && nWritten == nSize;
    }

    public static IEnumerable<Module> Modules(this Process process)
    {
        return Modules(process.Handle);
    }

    public static IEnumerable<Module> Modules(nint processHandle)
    {
        if (!EnumProcessModulesEx(processHandle, null, 0, out uint cbNeeded))
        {
            ThrowHelper.ThrowWin32Exception();
        }

        int numModules = (int)(cbNeeded / Unsafe.SizeOf<nint>());
        nint[] hModule = ArrayPoolExtensions.Rent<nint>(numModules);

        if (!EnumProcessModulesEx(processHandle, hModule, cbNeeded, out _))
        {
            ThrowHelper.ThrowWin32Exception();
        }

        for (int i = 0; i < numModules; i++)
        {
            if (!GetModuleBaseNameW(processHandle, hModule[i], out string baseName))
            {
                ArrayPoolExtensions.ReturnIfNotNull(hModule);

                yield break;
            }

            if (!GetModuleFileNameExW(processHandle, hModule[i], out string fileName))
            {
                ArrayPoolExtensions.ReturnIfNotNull(hModule);

                yield break;
            }

            if (!GetModuleInformation(processHandle, hModule[i], out MODULEINFO moduleInfo))
            {
                ArrayPoolExtensions.ReturnIfNotNull(hModule);

                yield break;
            }

            yield return new(processHandle, baseName, fileName, moduleInfo);
        }
    }

    public static IEnumerable<Module> ModulesTh32(this Process process)
    {
        return ModulesTh32(process.Handle, process.Id);
    }

    public static IEnumerable<Module> ModulesTh32(nint processHandle, int processId)
    {
        MODULEENTRY32W me = new() { dwSize = MODULEENTRY32W.Size };
        nint snapshot = CreateToolhelp32Snapshot(ThFlags.TH32CS_SNAPMODULE | ThFlags.TH32CS_SNAPMODULE32, processId);

        try
        {
            if (!Module32FirstW(snapshot, ref me))
            {
                ThrowHelper.ThrowWin32Exception();
            }

            do
            {
                yield return new(processHandle, me);
            } while (Module32NextW(snapshot, ref me));
        }
        finally
        {
            _ = CloseHandle(snapshot);
        }
    }

    public static IEnumerable<MemoryPage> MemoryPages(this Process process, bool allPages)
    {
        nint processHandle = process.Handle;
        return MemoryPages(processHandle, Is64Bit(processHandle), allPages);
    }

    public static IEnumerable<MemoryPage> MemoryPages(this Process process, bool is64Bit, bool allPages)
    {
        return MemoryPages(process.Handle, is64Bit, allPages);
    }

    public static IEnumerable<MemoryPage> MemoryPages(nint processHandle, bool is64Bit, bool allPages)
    {
        nint addr = 0x10000, max = (nint)(is64Bit ? 0x7FFFFFFEFFFF : 0x7FFEFFFF);

        while (VirtualQueryEx(processHandle, addr, out MEMORY_BASIC_INFORMATION mbi))
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
        return Symbols(module, process.Handle);
    }

    public static Dictionary<string, DebugSymbol> Symbols(this Module module, nint processHandle)
    {
        ThrowHelper.ThrowIfNull(module);

        Dictionary<string, DebugSymbol> syms = new(StringComparer.OrdinalIgnoreCase);
        void* pSyms = Unsafe.AsPointer(ref syms);

        getSymbols(null);
        getSymbols(Path.GetDirectoryName(module.FilePath));

        return syms;

        void getSymbols(string pdbDirectory)
        {
            if (!SymInitializeW(processHandle, pdbDirectory))
            {
                ThrowHelper.ThrowWin32Exception();
            }

            try
            {
                if (!SymLoadModuleExW(processHandle, module))
                {
                    ThrowHelper.ThrowWin32Exception();
                }

                if (!SymEnumSymbolsW(processHandle, module, pSyms))
                {
                    ThrowHelper.ThrowWin32Exception();
                }
            }
            finally
            {
                _ = SymCleanup((void*)processHandle);
            }
        }
    }

    public static nint AllocateRemoteString(this Process process, string value)
    {
        return AllocateRemoteString(process.Handle, value);
    }

    public static nint AllocateRemoteString(nint processHandle, string value)
    {
        void* hProcess = (void*)processHandle;

        if (hProcess == null)
        {
            ThrowHelper.ThrowArgumentNullException(nameof(hProcess));
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
            ThrowHelper.ThrowWin32Exception();
        }

        fixed (char* pValue = value)
        {
            nuint nWritten;

            if (WriteProcessMemory(hProcess, memory, pValue, length, &nWritten) == 0
                || nWritten != length)
            {
                ThrowHelper.ThrowWin32Exception();
            }
        }

        return (nint)memory;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNativeIntAndZero<T>(T value)
    {
        if (typeof(T) == typeof(nint))
        {
            nint nValue = Unsafe.As<T, nint>(ref value);
            return nValue == 0;
        }
        else if (typeof(T) == typeof(nuint))
        {
            nuint nuValue = Unsafe.As<T, nuint>(ref value);
            return nuValue == 0;
        }
        else
        {
            return false;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNativeInt<T>()
    {
        return typeof(T) == typeof(nint) || typeof(T) == typeof(nuint);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetTypeSize<T>(bool is64Bit)
        where T : unmanaged
    {
        return GetTypeSize<T>((byte)(is64Bit ? 0x8 : 0x4));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetTypeSize<T>(byte ptrSize)
        where T : unmanaged
    {
        return IsNativeInt<T>() ? ptrSize : sizeof(T);
    }
}
