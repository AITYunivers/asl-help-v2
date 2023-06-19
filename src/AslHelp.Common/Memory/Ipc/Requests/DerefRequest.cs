namespace AslHelp.Common.Memory.Ipc.Requests;

public unsafe record struct DerefRequest(
    PipeRequest Code,
    nuint BaseAddress,
    int OffsetCount,
    int* Offsets);
