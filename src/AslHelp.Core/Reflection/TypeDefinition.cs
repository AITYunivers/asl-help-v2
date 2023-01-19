namespace AslHelp.Core.Reflection;

public sealed class TypeDefinition<T>
    : ITypeDefinition<T>
    where T : unmanaged
{
    public T Default { get; } = default;
    public unsafe int Size { get; } = sizeof(T);

    object ITypeDefinition.Default => Default;

    public unsafe T Convert(byte* buffer)
    {
        return Unsafe.ReadUnaligned<T>(buffer);
    }

    unsafe object ITypeDefinition.Convert(byte* buffer)
    {
        return Convert(buffer);
    }
}
