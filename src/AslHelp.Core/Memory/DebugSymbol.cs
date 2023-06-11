using AslHelp.Core.Memory.Native.Structs;

namespace AslHelp.Core.Memory;

public readonly record struct DebugSymbol
{
    internal unsafe DebugSymbol(SYMBOL_INFOW symbol)
    {
        Name = new((char*)symbol.Name);
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
