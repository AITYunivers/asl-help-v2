namespace AslHelp.Core.Reflection;

public sealed class TypeDefinition
{
    public TypeDefinition(Type type)
    {
        Type = type;
        Size = Marshal.SizeOf(type);
        Default = Activator.CreateInstance(type);
    }

    public Type Type { get; }
    public int Size { get; }

    public object Default { get; }
}
