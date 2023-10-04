using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AslHelp.Core.IO.Parsing;

public sealed class NativeField
{
    private readonly int _trailingZeroCount;

    public NativeField(BitField? bitField)
    {
        if (bitField is (int size, int offset))
        {
            BitMask = ((1u << size) - 1) << offset;
            _trailingZeroCount = TrailingZeroCount(BitMask);
        }
    }

    public required string Name { get; init; }
    public required string Type { get; init; }
    public required uint Offset { get; init; }
    public required uint Size { get; init; }
    public required uint Alignment { get; init; }

    public uint BitMask { get; }

    public static implicit operator uint(NativeField field)
    {
        return field.Offset;
    }

    public static byte operator &(byte value, NativeField field)
    {
        return (byte)((value & field.BitMask) >> field._trailingZeroCount);
    }

    public static sbyte operator &(sbyte value, NativeField field)
    {
        return (sbyte)((value & field.BitMask) >> field._trailingZeroCount);
    }

    public static ushort operator &(ushort value, NativeField field)
    {
        return (ushort)((value & field.BitMask) >> field._trailingZeroCount);
    }

    public static short operator &(short value, NativeField field)
    {
        return (short)((value & field.BitMask) >> field._trailingZeroCount);
    }

    public static uint operator &(uint value, NativeField field)
    {
        return (value & field.BitMask) >> field._trailingZeroCount;
    }

    public static int operator &(int value, NativeField field)
    {
        return (int)((value & field.BitMask) >> field._trailingZeroCount);
    }

    public static ulong operator &(ulong value, NativeField field)
    {
        return (value & field.BitMask) >> field._trailingZeroCount;
    }

    public static long operator &(long value, NativeField field)
    {
        return (value & field.BitMask) >> field._trailingZeroCount;
    }

    // https://learn.microsoft.com/en-us/dotnet/api/System.Numerics.BitOperations.TrailingZeroCount
    private static int TrailingZeroCount(uint mask)
    {
        return Unsafe.AddByteOffset(
            ref MemoryMarshal.GetReference(TrailingZeroCountDeBruijn),
            (nint)(int)(((mask & (uint)-(int)mask) * 0x077CB531u) >> 27));
    }

    private static ReadOnlySpan<byte> TrailingZeroCountDeBruijn => new byte[]
    {
        00, 01, 28, 02, 29, 14, 24, 03,
        30, 22, 20, 15, 25, 17, 04, 08,
        31, 27, 13, 23, 21, 19, 16, 07,
        26, 12, 18, 06, 11, 05, 10, 09
    };
}
