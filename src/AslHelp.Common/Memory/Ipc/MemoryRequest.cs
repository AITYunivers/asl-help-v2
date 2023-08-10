namespace AslHelp.Common.Memory.Ipc;

/// <summary>
///     The <see cref="PipeRequest"/> enum
///     represents different possible requests made to a named pipe.
/// </summary>
public enum PipeRequest
{
    /// <summary>
    ///     Requests that the pipe be closed.
    /// </summary>
    Close,

    /// <summary>
    ///     Indicates that the following data will be a <see cref="DerefRequest"/>.
    /// </summary>
    Deref,

    /// <summary>
    ///     Indicates that the following data will be a <see cref="ReadRequest"/>.
    /// </summary>
    Read,

    /// <summary>
    ///     Indicates that the following data will be a <see cref="WriteRequest"/>.
    /// </summary>
    Write
}
