namespace AslHelp.Common.Memory.Ipc;

public unsafe record struct DerefRequest(
    nuint BaseAddress,
    ulong Offsets,
    uint OffsetsLength,
    ulong ResultPtr);
