namespace AslHelp.Mono.Memory.MonoInterop.MonoV1.Structs;

internal readonly struct Slot32
{
    public readonly uint key;
    public readonly uint next;
    public readonly uint prev;
}

internal readonly struct Slot64
{
    public readonly ulong key;
    public readonly ulong next;
    public readonly ulong prev;
}
