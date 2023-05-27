namespace AslHelp.Core.Memory.Models;

public readonly record struct DebugSymbol
{
    internal unsafe DebugSymbol(SYMBOL_INFOW symbol)
    {
        Name = new((char*)symbol.Name);
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
