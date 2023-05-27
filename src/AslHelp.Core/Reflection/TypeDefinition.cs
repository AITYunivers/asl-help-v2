using System.Runtime.CompilerServices;

namespace AslHelp.Core.Reflection;

public sealed class TypeDefinition<T> : ITypeDefinition<T>
    where T : unmanaged
{
    public T Default { get; } = default;
    public unsafe int Size { get; } = sizeof(T);

    object ITypeDefinition.Default => Default;

    public unsafe T CreateInstance(byte* buffer)
    {
        return Unsafe.ReadUnaligned<T>(buffer);
    }

    unsafe object ITypeDefinition.CreateInstance(byte* buffer)
    {
        return CreateInstance(buffer);
    }
}
