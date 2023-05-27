namespace AslHelp.Core.Reflection;

public interface ITypeDefinition
{
    int Size { get; }
    object Default { get; }

    unsafe object CreateInstance(byte* buffer);
}

public interface ITypeDefinition<T> : ITypeDefinition
{
    new T Default { get; }

    new unsafe T CreateInstance(byte* buffer);
}
