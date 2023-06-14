using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using AslHelp.Core.Memory.Native.Structs;

namespace AslHelp.Core.Memory;

public sealed class Module
{
    private readonly nuint _processHandle;

    public Module(nint processHandle, string name, string fileName, nint @base, int memorySize)
    {
        _processHandle = processHandle;

        Name = name;
        FileName = fileName;
        Base = @base;
        MemorySize = memorySize;
    }

    internal unsafe Module(nint processHandle, string baseName, string fileName, MODULEINFO moduleInfo)
    {
        _processHandle = (nuint)processHandle;

        Name = baseName;
        FileName = fileName;
        Base = (nint)moduleInfo.lpBaseOfDll;
        MemorySize = (int)moduleInfo.SizeOfImage;
    }

    internal unsafe Module(nint processHandle, MODULEENTRY32W me)
    {
        _processHandle = (nuint)processHandle;

        Name = new((char*)me.szModule);
        FileName = new((char*)me.szExePath);
        Base = (nint)me.modBaseAddr;
        MemorySize = (int)me.modBaseSize;
    }

    public string Name { get; }
    public string FileName { get; }
    public nint Base { get; }
    public int MemorySize { get; }

    private Dictionary<string, DebugSymbol>? _symbols;
    public Dictionary<string, DebugSymbol> Symbols
    {
        get
        {
            if (_symbols is not null)
            {
                return _symbols;
            }

            _symbols = new(StringComparer.OrdinalIgnoreCase);

            List<SYMBOL_INFOW> nonPdbSymbols = this.GetSymbols(_processHandle, "*", null);
            List<SYMBOL_INFOW> pdbSymbols = this.GetSymbols(_processHandle, "*", Path.GetDirectoryName(FileName));

            for (int i = 0; i < nonPdbSymbols.Count; i++)
            {
                DebugSymbol symbol = new(nonPdbSymbols[i]);
                _symbols[symbol.Name] = symbol;
            }

            for (int i = 0; i < pdbSymbols.Count; i++)
            {
                DebugSymbol symbol = new(pdbSymbols[i]);
                _symbols[symbol.Name] = symbol;
            }

            return _symbols;
        }
    }

    public FileVersionInfo VersionInfo => FileVersionInfo.GetVersionInfo(FileName);

    public override string ToString()
    {
        return Name;
    }
}
