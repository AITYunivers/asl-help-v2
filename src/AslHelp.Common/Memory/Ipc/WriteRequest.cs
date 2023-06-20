namespace AslHelp.Common.Memory.Ipc;

public unsafe record struct WriteRequest(
    nuint BaseAddress,
    ulong Offsets,
    uint OffsetsLength,
    ulong Data,
    uint DataLength);
