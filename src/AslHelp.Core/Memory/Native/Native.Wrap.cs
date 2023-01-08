using AslHelp.Core.Memory.Models;

namespace AslHelp.Core.Memory;

#pragma warning disable IDE1006
internal static unsafe partial class Native
{
    private const int MAX_PATH = 260;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool EnumProcessModulesEx(nint hProcess, ReadOnlySpan<nint> hModule, uint cb, out uint cbNeeded)
    {
        fixed (void* lphModule = hModule)
        fixed (uint* lpcbNeeded = &cbNeeded)
        {
            return K32EnumProcessModulesEx(
                (void*)hProcess,
                (void**)lphModule,
                cb,
                lpcbNeeded,
                LIST_MODULES_ALL) != 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool GetModuleBaseNameW(nint hProcess, nint hModule, out string baseName)
    {
        ushort* buffer = stackalloc ushort[MAX_PATH];

        if (K32GetModuleBaseNameW((void*)hProcess, (void*)hModule, buffer, MAX_PATH) == 0)
        {
            baseName = null;
            return false;
        }

        baseName = new((char*)buffer);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool GetModuleFileNameExW(nint hProcess, nint hModule, out string fileName)
    {
        ushort* buffer = stackalloc ushort[MAX_PATH];

        if (K32GetModuleFileNameExW((void*)hProcess, (void*)hModule, buffer, MAX_PATH) == 0)
        {
            fileName = null;
            return false;
        }

        fileName = new((char*)buffer);
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool GetModuleInformation(nint hProcess, nint hModule, out MODULEINFO moduleInfo)
    {
        fixed (MODULEINFO* lpmodinfo = &moduleInfo)
        {
            return K32GetModuleInformation((void*)hProcess, (void*)hModule, lpmodinfo, MODULEINFO.Size) != 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool VirtualQueryEx(nint hProcess, nint address, out MEMORY_BASIC_INFORMATION mbi)
    {
        fixed (MEMORY_BASIC_INFORMATION* lpBuffer = &mbi)
        {
            return VirtualQueryEx((void*)hProcess, (void*)address, lpBuffer, MEMORY_BASIC_INFORMATION.Size) != 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SymInitializeW(nint hProcess, string userSearchPath)
    {
        fixed (char* lpUserSearchPath = userSearchPath)
        {
            return SymInitializeW((void*)hProcess, (ushort*)lpUserSearchPath, 0) != 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SymLoadModuleExW(nint hProcess, Module module)
    {
        fixed (char* lpImageName = module.Name)
        {
            return SymLoadModuleExW((void*)hProcess, (void*)0, (ushort*)lpImageName, null, (ulong)module.Base, (uint)module.MemorySize, (void*)0, 0) != 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SymEnumSymbolsW(nint hProcess, Module module, void* pSymbols)
    {
        nint callbackPtr = Marshal.GetFunctionPointerForDelegate(EnumSymbolsCallback);
        ushort* mask = stackalloc ushort[2] { '*', '\0' };

        return SymEnumSymbolsW(
            (void*)hProcess,
            (ulong)module.Base,
            mask,
            (delegate* unmanaged[Stdcall]<SYMBOL_INFOW*, uint, void*, int>)callbackPtr,
            pSymbols) != 0;
    }

    private static int EnumSymbolsCallback(SYMBOL_INFOW* pSymInfo, uint SymbolSize, void* UserContext)
    {
        Unsafe.AsRef<List<DebugSymbol>>(UserContext).Add(new(*pSymInfo));
        return 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static nint CreateToolhelp32Snapshot(ThFlags dwFlags, int th32ProcessID)
    {
        return (nint)CreateToolhelp32Snapshot(dwFlags, (uint)th32ProcessID);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Module32FirstW(nint hSnapshot, ref MODULEENTRY32W me)
    {
        fixed (MODULEENTRY32W* lpme = &me)
        {
            return Module32FirstW((void*)hSnapshot, lpme) != 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Module32NextW(nint hSnapshot, ref MODULEENTRY32W me)
    {
        fixed (MODULEENTRY32W* lpme = &me)
        {
            return Module32NextW((void*)hSnapshot, lpme) != 0;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool CloseHandle(nint hObject)
    {
        return CloseHandle((void*)hObject) != 0;
    }
}
#pragma warning restore IDE1006
