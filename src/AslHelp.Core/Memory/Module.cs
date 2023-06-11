using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

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

    private Dictionary<string, DebugSymbol>? _symbols;
    public unsafe Dictionary<string, DebugSymbol> Symbols
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
                addSymbol(nonPdbSymbols[i]);
            }

            for (int i = 0; i < pdbSymbols.Count; i++)
            {
                addSymbol(pdbSymbols[i]);
            }

            return _symbols;

            void addSymbol(SYMBOL_INFOW symbol)
            {
                ReadOnlySpan<char> name = new(symbol.Name, (int)symbol.NameLen);
                _symbols[name.ToString()] = new(symbol);
            }
        }
    }

    public FileVersionInfo VersionInfo => FileVersionInfo.GetVersionInfo(FileName);

    public override string ToString()
    {
        return Name;
    }
}
