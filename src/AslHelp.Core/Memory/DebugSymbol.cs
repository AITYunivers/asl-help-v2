using System;

using AslHelp.Core.Memory.Native.Structs;

namespace AslHelp.Core.Memory;

public readonly record struct DebugSymbol
{
    public DebugSymbol(string name, nuint address, uint size)
    {
        Name = name;
        Address = address;
        Size = size;
    }

    internal unsafe DebugSymbol(SYMBOL_INFOW symbol)
    {
        ReadOnlySpan<char> name = new((char*)symbol.Name, (int)symbol.NameLen - 1);

        Name = name.ToString();
        Address = (nuint)symbol.Address;
        Size = symbol.Size;
    }

    public string Name { get; }
    public nuint Address { get; }
    public uint Size { get; }

    public override string ToString()
    {
        return Name;
    }
}
