namespace AslHelp.Mono.Memory.MonoInterop.MonoV1.Structs;

internal readonly struct GHashTable32
{
    public readonly uint hash_func;
    public readonly uint key_equal_func;
    public readonly uint table;
    public readonly int table_size;
}

internal readonly struct GHashTable64
{
    public readonly ulong hash_func;
    public readonly ulong key_equal_func;
    public readonly ulong table;
    public readonly int table_size;
}
