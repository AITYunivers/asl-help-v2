namespace AslHelp.Common.Memory.Ipc;

/// <summary>
///     The <see cref="WriteRequest"/> structure
///     represents a request to write a value to a remote process.
/// </summary>
/// <param name="BaseAddress">The base address of the value to write.</param>
/// <param name="Offsets">A pointer to the offsets to apply to the pointer path.</param>
/// <param name="OffsetsLength">The number of offsets in the <paramref name="Offsets"/> array.</param>
/// <param name="Data">A pointer to the buffer containing the data to write.</param>
/// <param name="DataLength">The length of the buffer pointed to by <paramref name="Data"/>.</param>
public unsafe record struct WriteRequest(
    ulong BaseAddress,
    ulong Offsets,
    uint OffsetsLength,
    ulong Data,
    uint DataLength);
