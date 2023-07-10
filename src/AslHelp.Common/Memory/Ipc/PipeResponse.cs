namespace AslHelp.Common.Memory.Ipc;

public enum PipeResponse
{
    Success,

    ReceiveFailure,
    DerefFailure,
    ReadFailure,
    WriteFailure,

    PipeClosed,
    UnknownCommand
}
