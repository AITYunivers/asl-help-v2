namespace AslHelp.Common.Memory.Ipc;

/// <summary>
///     The <see cref="PipeResponse"/> enum
///     represents different possible responses from a named pipe.
/// </summary>
public enum PipeResponse
{
    /// <summary>
    ///     Indicates that the request was successful.
    /// </summary>
    Success,

    /// <summary>
    ///     Indicates that the data could not be read from the pipe.
    /// </summary>
    ReceiveFailure,

    /// <summary>
    ///     Indicates the pointer path could not be dereferenced successfully.
    /// </summary>
    DerefFailure,

    /// <summary>
    ///     Indicates that reading the requested value failed.
    /// </summary>
    ReadFailure,

    /// <summary>
    ///     Indicates that writing the requested value failed.
    /// </summary>
    WriteFailure,

    /// <summary>
    ///     Indicates that the pipe was closed.
    /// </summary>
    PipeClosed,

    /// <summary>
    ///     Indicates that the request was not recognized.
    /// </summary>
    UnknownCommand
}
