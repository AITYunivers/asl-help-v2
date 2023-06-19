namespace AslHelp.Common.Memory.Ipc.Requests;

public unsafe record struct ReadRequest(
    PipeRequest Code,
    nuint BaseAddress,
    int OffsetCount,
    int* Offsets,
    int Size,
    void* Buffer);
