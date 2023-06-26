namespace AslHelp.Common.Memory.Ipc;

public enum PipeResponse
{
    Success,

    ReceiveFailure,
    DerefFailure,

    PipeClosed,
    UnknownCommand
}
