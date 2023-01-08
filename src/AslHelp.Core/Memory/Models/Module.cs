namespace AslHelp.Core.Memory.Models;

public record Module
{
    internal unsafe Module(MODULEENTRY32W me)
    {
        Name = new(me.szModule);
        FilePath = new(me.szExePath);
        Base = (nint)me.modBaseAddr;
        MemorySize = (int)me.modBaseSize;
    }

    internal unsafe Module(string baseName, string fileName, MODULEINFO moduleInfo)
    {
        Name = baseName;
        FilePath = fileName;
        Base = (nint)moduleInfo.lpBaseOfDll;
        MemorySize = (int)moduleInfo.SizeOfImage;
        //Symbols = new(this);
    }

    public string Name { get; }
    public string FilePath { get; }
    public nint Base { get; }
    public int MemorySize { get; }
    //public SymbolCache Symbols { get; }

    public FileVersionInfo VersionInfo => FileVersionInfo.GetVersionInfo(FilePath);

    public override string ToString()
    {
        return Name;
    }
}
