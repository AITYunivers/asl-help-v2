using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AslHelp.Common.Memory.Ipc;

/// <summary />
[SkipLocalsInit]
[StructLayout(LayoutKind.Explicit)]
public unsafe struct ReadRequest
{
    /// <summary>
    /// ///     The base address of the value to read.

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

    /// <summary>
    ///     The size of the data to be read, in bytes.
    /// </summary>
    [FieldOffset(0x20C)]
    public uint BufferLength;

    public IMemoryOwner
}
