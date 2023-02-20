namespace AslHelp.Core.Memory.Models;

public sealed record Module
{
    private readonly nint _processHandle;

    internal Module(Process process, string baseName, string fileName, MODULEINFO moduleInfo)
        : this(process.Handle, baseName, fileName, moduleInfo) { }

    internal unsafe Module(nint processHandle, string baseName, string fileName, MODULEINFO moduleInfo)
    {
        _processHandle = processHandle;

        Name = baseName;
        FilePath = fileName;
        Base = (nint)moduleInfo.lpBaseOfDll;
        MemorySize = (int)moduleInfo.SizeOfImage;
    }

    internal Module(Process process, MODULEENTRY32W me)
        : this(process.Handle, me) { }

    internal unsafe Module(nint processHandle, MODULEENTRY32W me)
    {
        _processHandle = processHandle;

        Name = new((char*)me.szModule);
        FilePath = new((char*)me.szExePath);
        Base = (nint)me.modBaseAddr;
        MemorySize = (int)me.modBaseSize;
    }

    public string Name { get; }
    public string FilePath { get; }
    public nint Base { get; }
    public int MemorySize { get; }

    private Dictionary<string, DebugSymbol> _symbols;
    public Dictionary<string, DebugSymbol> Symbols => _symbols ??= this.Symbols(_processHandle);

    public FileVersionInfo VersionInfo => FileVersionInfo.GetVersionInfo(FilePath);

    public override string ToString()
    {
        return Name;
    }
}
