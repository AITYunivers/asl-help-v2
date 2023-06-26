namespace AslHelp.Common.Memory.Ipc;

public unsafe record struct DerefRequest(
    ulong BaseAddress,
    ulong Offsets,
    uint OffsetsLength,
    ulong ResultPtr);
