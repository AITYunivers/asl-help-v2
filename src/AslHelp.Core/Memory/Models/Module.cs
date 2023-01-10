using AslHelp.Core.Collections;

namespace AslHelp.Core.Memory.Models;

public record Module
{
    internal unsafe Module(Process process, MODULEENTRY32W me)
    {
        Name = new(me.szModule);
        FilePath = new(me.szExePath);
        Base = (nint)me.modBaseAddr;
        MemorySize = (int)me.modBaseSize;
        Symbols = new(this, process);
    }

    internal unsafe Module(Process process, string baseName, string fileName, MODULEINFO moduleInfo)
    {
        Name = baseName;
        FilePath = fileName;
        Base = (nint)moduleInfo.lpBaseOfDll;
        MemorySize = (int)moduleInfo.SizeOfImage;
        Symbols = new(this, process);
    }

    public string Name { get; }
    public string FilePath { get; }
    public nint Base { get; }
    public int MemorySize { get; }
    public SymbolCollection Symbols { get; }

    public FileVersionInfo VersionInfo => FileVersionInfo.GetVersionInfo(FilePath);

    public override string ToString()
    {
        return Name;
    }
}
