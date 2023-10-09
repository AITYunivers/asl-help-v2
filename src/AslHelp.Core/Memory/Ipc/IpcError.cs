using AslHelp.Common.Results;

namespace AslHelp.Core.Memory.Ipc;

public enum IpcError
{
    [ErrorMessage("Cannot interact with the memory of an exited process.")]
    MemoryIsDisposed,

    [ErrorMessage("Base address was null.")]
    BaseAddressIsNullPtr,

    [ErrorMessage("Failure during dereferencing.")]
    DerefFailure,

    [ErrorMessage("Failure during memory read.")]
    ReadFailure,

    [ErrorMessage("Failure during memory write.")]
    WriteFailure,

    [ErrorMessage("Cannot dereference more than 128 offsets.")]
    OffsetsLengthIsGreaterThan128,

    [ErrorMessage("Unable to receive pipe response.")]
    ReceiveFailure
}
