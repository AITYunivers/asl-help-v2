using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.Native.Enums;
using AslHelp.Core.Memory.Native.Structs;

namespace AslHelp.Core.Memory.Native;

internal static unsafe partial class WinInteropWrapper
{
    public static bool ProcessIs64Bit(this Process process)
    {
        return ProcessIs64Bit((nuint)(nint)process.Handle);
    }

    public static bool ProcessIs64Bit(nuint processHandle)
    {
        if (!WinInterop.IsWow64Process(processHandle, out bool isWow64))
        {
            ThrowHelper.ThrowWin32Exception();
        }

        return Environment.Is64BitOperatingSystem && !isWow64;
    }

    public static bool ReadMemory(this Process process, nuint address, void* buffer, uint bufferSize)
    {
        return ReadMemory((nuint)(nint)process.Handle, address, buffer, bufferSize);
    }

    public static bool ReadMemory(nuint processHandle, nuint address, void* buffer, uint bufferSize)
    {
        return WinInterop.ReadProcessMemory(processHandle, address, buffer, bufferSize, out nuint nRead)
            && nRead == bufferSize;
    }

    public static bool WriteMemory(this Process process, nuint address, void* data, uint dataSize)
    {
        return WriteMemory((nuint)(nint)process.Handle, address, data, dataSize);
    }

    public static bool WriteMemory(nuint processHandle, nuint address, void* data, uint dataSize)
    {
        return WinInterop.WriteProcessMemory(processHandle, address, data, dataSize, out nuint nWritten)
            && nWritten == dataSize;
    }

    public static IEnumerable<MODULEENTRY32W> EnumerateModulesTh32(this Process process)
    {
        return EnumerateModulesTh32((uint)process.Id);
    }

    public static IEnumerable<MODULEENTRY32W> EnumerateModulesTh32(uint processId)
    {
        nuint snapshot = WinInterop.CreateToolhelp32Snapshot(processId, ThFlags.TH32CS_SNAPMODULE | ThFlags.TH32CS_SNAPMODULE32);

        try
        {
            MODULEENTRY32W me = new() { dwSize = MODULEENTRY32W.Size };
            if (!WinInterop.Module32First(snapshot, ref me))
            {
                yield break;
            }

            do
            {
                yield return me;
            } while (WinInterop.Module32Next(snapshot, ref me));
        }
        finally
        {
            WinInterop.CloseHandle(snapshot);
        }
    }

    public static IEnumerable<MemoryPage> EnumerateMemoryPages(this Process process, bool allPages)
    {
        return EnumerateMemoryPages((nuint)(nint)process.Handle, process.ProcessIs64Bit(), allPages);
    }

    public static IEnumerable<MemoryPage> EnumerateMemoryPages(nuint processHandle, bool is64Bit, bool allPages)
    {
        nuint address = 0x10000, max = (nuint)(is64Bit ? 0x7FFFFFFEFFFF : 0x7FFEFFFF);

        do
        {
            if (WinInterop.VirtualQuery(processHandle, address, out MEMORY_BASIC_INFORMATION mbi) == 0)
            {
                break;
            }

            address += mbi.RegionSize;

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
        } while (address < max);
    }

    public static List<SYMBOL_INFOW> GetSymbols(this Module module, nuint processHandle, string? mask = "*", string? pdbDirectory = null)
    {
        var callback =
            (delegate* unmanaged[Stdcall]<SYMBOL_INFOW*, uint, void*, int>)Marshal.GetFunctionPointerForDelegate(enumSymbolsCallback);

        List<SYMBOL_INFOW> symbols = new();
        void* pSymbols = Unsafe.AsPointer(ref symbols);

        if (!WinInterop.SymInitialize(processHandle, pdbDirectory, false))
        {
            ThrowHelper.ThrowWin32Exception();
        }

        try
        {
            nuint symLoadBase =
                WinInterop.SymLoadModule(processHandle, 0, module.FileName, null, module.Base, module.MemorySize, null, 0);

            if (symLoadBase == 0)
            {
                ThrowHelper.ThrowWin32Exception();
            }

            if (!WinInterop.SymEnumSymbols(processHandle, symLoadBase, mask, callback, pSymbols))
            {
                ThrowHelper.ThrowWin32Exception();
            }
        }
        finally
        {
            _ = WinInterop.SymCleanup(processHandle);
        }

        return symbols;

        static int enumSymbolsCallback(SYMBOL_INFOW* pSymInfo, uint symbolSize, void* userContext)
        {
            Unsafe.AsRef<List<SYMBOL_INFOW>>(userContext).Add(*pSymInfo);

            return 1;
        }
    }
}
