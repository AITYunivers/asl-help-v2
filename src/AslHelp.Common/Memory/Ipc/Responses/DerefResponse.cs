namespace AslHelp.Common.Memory.Ipc.Responses;

public unsafe record struct DerefResponse(
    PipeResponse Code,
    nuint Result);
