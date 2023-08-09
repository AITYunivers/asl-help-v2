using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AslHelp.Common.Memory.Ipc;

/// <summary>
///     The <see cref="DerefRequest"/> structure
///     represents a request to dereference a pointer path in a remote process.
/// </summary>
[SkipLocalsInit]
[StructLayout(LayoutKind.Explicit)]
public unsafe struct DerefRequest
{
    /// <summary>
    ///     The base address of the pointer path to dereference.
    /// </summary>
    [FieldOffset(0x0)]
    public nuint BaseAddress;

    /// <summary>
    ///     The number of offsets in the <see cref="Offsets"/> array.
    /// </summary>
    [FieldOffset(0x8)]
    public int OffsetsLength;

    /// <summary>
    ///     A pointer to the offsets to apply to the pointer path.
    /// </summary>
    [FieldOffset(0xC)]
    public fixed int Offsets[128];
}
