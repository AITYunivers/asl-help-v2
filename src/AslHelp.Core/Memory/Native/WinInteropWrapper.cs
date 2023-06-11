using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Native;
using AslHelp.Core.Memory.Native.Enums;
using AslHelp.Core.Memory.Native.Structs;

namespace AslHelp.Core;

internal static unsafe class WinInteropWrapper
{
    public static IEnumerable<MODULEENTRY32W> EnumerateModulesTh32(this Process process)
    {
        return EnumerateModulesTh32((uint)process.Id);
    }

    public static IEnumerable<MODULEENTRY32W> EnumerateModulesTh32(uint processId)
    {
        nuint snapshot = WinInterop.CreateToolhelp32Snapshot(processId, ThFlags.TH32CS_SNAPMODULE | ThFlags.TH32CS_SNAPMODULE32);

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
            nuint symLoadBase = WinInterop.SymLoadModule(processHandle, 0, module.FileName, null, module.Base, module.MemorySize, null, 0);

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
