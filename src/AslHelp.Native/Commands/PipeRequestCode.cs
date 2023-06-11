namespace AslHelp.Native.Commands;

public enum PipeRequestCode : byte
{
    ClosePipe,

    Deref,
    Read,
    ReadSpan,
    Write,
    WriteSpan
}
