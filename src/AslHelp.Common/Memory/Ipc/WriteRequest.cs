namespace AslHelp.Common.Memory.Ipc;

public unsafe record struct WriteRequest(
    ulong BaseAddress,
    ulong Offsets,
    uint OffsetsLength,
    ulong Data,
    uint DataLength);
