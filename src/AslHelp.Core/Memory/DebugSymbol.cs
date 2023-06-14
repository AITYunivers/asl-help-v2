using System;

using AslHelp.Core.Memory.Native.Structs;

namespace AslHelp.Core.Memory;

public readonly record struct DebugSymbol
{
    public DebugSymbol(string name, nint address, int size)
    {
        Name = name;
        Address = address;
        Size = size;
    }

    internal unsafe DebugSymbol(SYMBOL_INFOW symbol)
    {
        ReadOnlySpan<char> name = new((char*)symbol.Name, (int)symbol.NameLen);

        Name = name.ToString();
        Address = (nint)symbol.Address;
        Size = (int)symbol.Size;
    }

    public string Name { get; }
    public nint Address { get; }
    public int Size { get; }

    public override string ToString()
    {
        return Name;
    }
}
