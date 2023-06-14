namespace AslHelp.Core.Reflection;

public interface ITypeDefinition
{
    int Size { get; }
    object Default { get; }

    unsafe object CreateInstance(void* buffer);
}

public interface ITypeDefinition<T> : ITypeDefinition
    where T : unmanaged
{
    new T Default { get; }

    new unsafe T CreateInstance(void* buffer);
}
