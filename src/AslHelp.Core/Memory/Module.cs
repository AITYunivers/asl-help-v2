using System.Collections.Generic;
using System.Diagnostics;

using AslHelp.Core.Memory.Native.Structs;

namespace AslHelp.Core.Memory;

public sealed class Module
{
    private readonly nuint _processHandle;

    internal Module(Process process, string baseName, string fileName, MODULEINFO moduleInfo)
        : this((nuint)(nint)process.Handle, baseName, fileName, moduleInfo) { }

    internal unsafe Module(nuint processHandle, string baseName, string fileName, MODULEINFO moduleInfo)
    {
        _processHandle = processHandle;

        Name = baseName;
        FileName = fileName;
        Base = (nuint)moduleInfo.lpBaseOfDll;
        MemorySize = moduleInfo.SizeOfImage;
    }

    internal Module(Process process, MODULEENTRY32W me)
        : this((nuint)(nint)process.Handle, me) { }

    internal unsafe Module(nuint processHandle, MODULEENTRY32W me)
    {
        _processHandle = processHandle;

        Name = new((char*)me.szModule);
        FileName = new((char*)me.szExePath);
        Base = (nuint)me.modBaseAddr;
        MemorySize = me.modBaseSize;
    }

    public string Name { get; }
    public string FileName { get; }
    public nuint Base { get; }
    public uint MemorySize { get; }

    // private Dictionary<string, DebugSymbol> _symbols;
    // public Dictionary<string, DebugSymbol> Symbols => _symbols ??= this.Symbols(_processHandle);

    public FileVersionInfo VersionInfo => FileVersionInfo.GetVersionInfo(FileName);

    public override string ToString()
    {
        return Name;
    }
}
