using System.Diagnostics;
using AslHelp.Core.Collections;

namespace AslHelp.Core.Memory.Models;

public record Module
{
    private readonly Process _process;

    internal unsafe Module(Process process, MODULEENTRY32W me)
    {
        _process = process;

        Name = new(me.szModule);
        FilePath = new(me.szExePath);
        Base = (nint)me.modBaseAddr;
        MemorySize = (int)me.modBaseSize;
    }

    internal unsafe Module(Process process, string baseName, string fileName, MODULEINFO moduleInfo)
    {
        _process = process;

        Name = baseName;
        FilePath = fileName;
        Base = (nint)moduleInfo.lpBaseOfDll;
        MemorySize = (int)moduleInfo.SizeOfImage;
    }

    public string Name { get; }
    public string FilePath { get; }
    public nint Base { get; }
    public int MemorySize { get; }

    private Dictionary<string, DebugSymbol> _symbols;
    public Dictionary<string, DebugSymbol> Symbols => _symbols ??= this.Symbols(_process);

    public FileVersionInfo VersionInfo => FileVersionInfo.GetVersionInfo(FilePath);

    public override string ToString()
    {
        return Name;
    }
}
