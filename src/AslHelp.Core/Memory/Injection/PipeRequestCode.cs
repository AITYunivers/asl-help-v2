namespace AslHelp.Core.Memory.Pipes;

internal enum PipeRequestCode
{
    ClosePipe,

    Deref,
    Read,
    ReadSpan,
    Write,
    WriteSpan
}
