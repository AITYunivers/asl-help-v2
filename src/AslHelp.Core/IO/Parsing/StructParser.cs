using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.IO.Parsing;

public partial class NativeStructMap
{
    private record Root(
        [property: JsonPropertyName("inherits")] Inheritance? Inheritance,
        [property: JsonPropertyName("signatures")] Signature[]? Signatures,
        [property: JsonPropertyName("structs")] Struct[]? Structs);

    private record Inheritance(
        [property: JsonPropertyName("major")] string Major,
        [property: JsonPropertyName("minor")] string Minor);

    private record Signature(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("offset")] int Offset,
        [property: JsonPropertyName("pattern")] string Pattern);

    private record Struct(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("super")] string? Super,
        [property: JsonPropertyName("fields")] Field[] Fields);

    private record Field(
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("alignment")] int Alignment);

    internal static NativeStructMap Parse(string engine, string major, string minor, bool is64Bit)
    {
        using Stream source = EmbeddedResource.GetResourceStream($"AslHelp.{engine}.Memory.Native.{major}-{minor}.jsonc");

        Root? root = JsonSerializer.Deserialize<Root>(source);
        if (root is null)
        {
            const string msg = "Provided resource was a JSON 'null' literal.";
            ThrowHelper.ThrowFormatException(msg);
        }

        if (root.Inheritance is null && root.Structs is null)
        {
            const string msg = "One of 'inherit' or 'structs' must be provided.";
            ThrowHelper.ThrowFormatException(msg);
        }

        NativeStructMap nsm =
            root.Inheritance is { Major.Length: > 0, Minor.Length: > 0 } inherit
            ? Parse(engine, inherit.Major, inherit.Minor, is64Bit)
            : new();

        if (root.Structs is not { Length: > 0 } structs)
        {
            return nsm;
        }

        foreach (Struct s in structs)
        {
            NativeStruct ns = new()
            {
                Name = s.Name,
                Super = s.Super
            };

            int offset = 0, alignment = 0;
            if (s.Super is not null && nsm.TryGetValue(s.Super, out var super))
            {
                NativeField last = super.Values.Last();

                offset = last.Offset + last.Size;
                alignment = last.Alignment;
            }

            foreach (Field f in s.Fields)
            {
                var x = f.Type switch
                {
                    [.., '*'] => 0,
                    [.. string type, '[', .. string foo, ']'] => 1,
                    [.. string type, '<', .., '>' ] => 2,
                    _ => 3
                };
            }
        }
    }
}

public sealed partial class NativeStructMap : Dictionary<string, NativeStruct> { }

public sealed class NativeStruct : Dictionary<string, NativeField>
{
    public required string Name { get; init; }
    public required string? Super { get; init; }

    public IEnumerable<NativeField> Fields => Values;
}

public sealed class NativeField
{
    private readonly int _trailingZeroCount;

    public NativeField() { }

    public NativeField(int bitFieldOffset, int bitFieldSize)
    {
        BitMask = ((1u << bitFieldSize) - 1) << bitFieldOffset;
        _trailingZeroCount = TrailingZeroCount(BitMask);
    }

    public required string Name { get; init; }
    public required string Type { get; init; }
    public required int Offset { get; init; }
    public required int Size { get; init; }
    public required int Alignment { get; init; }

    public uint BitMask { get; }

    public static implicit operator int(NativeField field)
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
