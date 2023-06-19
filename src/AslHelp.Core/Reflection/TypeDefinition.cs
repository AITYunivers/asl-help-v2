using System.Runtime.CompilerServices;

using AslHelp.Core.Reflection;

namespace AslHelp.Core;

public readonly struct TypeDefinition<T> : ITypeDefinition<T>
    where T : unmanaged
{
    public TypeDefinition() { }

    public T Default { get; } = default;
    public unsafe uint Size { get; } = (uint)sizeof(T);

    object ITypeDefinition.Default => Default;

    public unsafe T CreateInstance(void* buffer)
    {
        return Unsafe.ReadUnaligned<T>(buffer);
    }

    unsafe object ITypeDefinition.CreateInstance(void* buffer)
    {
        return CreateInstance(buffer);
    }
}
