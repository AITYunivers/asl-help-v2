namespace AslHelp.Common.Memory.Ipc;

public unsafe record struct ReadRequest(
    nuint BaseAddress,
    ulong Offsets,
    uint OffsetsLength,
    ulong Buffer,
    uint BufferLength);
