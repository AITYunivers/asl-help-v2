namespace AslHelp.Common.Memory.Ipc;

/// <summary>
///     The <see cref="DerefRequest"/> structure
///     represents a request to dereference a pointer path in a remote process.
/// </summary>
/// <param name="BaseAddress">The base address of the pointer path to dereference.</param>
/// <param name="Offsets">A pointer to the offsets to apply to the pointer path.</param>
/// <param name="OffsetsLength">The number of offsets in the <paramref name="Offsets"/> array.</param>
/// <param name="ResultPtr">A pointer to the buffer to which the result should be written.</param>
public unsafe record struct DerefRequest(
    ulong BaseAddress,
    ulong Offsets,
    uint OffsetsLength,
    ulong ResultPtr);
