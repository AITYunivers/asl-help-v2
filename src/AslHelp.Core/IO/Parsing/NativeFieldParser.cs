using System;
using System.Text.RegularExpressions;

using AslHelp.Common.Extensions;

namespace AslHelp.Core.IO.Parsing;

internal record struct FieldParseResult(
    string TypeName,
    int Offset,
    int Size,
    int Alignment,
    BitField? BitField);

public record struct BitField(
    int Size,
    int Offset);

internal sealed class NativeFieldParser
{
    private readonly Regex _genericRegex = new(@"^(\w+)<\w+>$", RegexOptions.Compiled);
    private readonly Regex _arrayRegex = new(@"^(\w+)\[(\d*)\]$", RegexOptions.Compiled);
    private readonly Regex _bitfieldRegex = new(@"^(\w+):(\d+)$", RegexOptions.Compiled);

    private readonly NativeStructMap _structs;
    private readonly byte _ptrSize;

    private int _fieldOffset;
    private int _bitOffset = -1;

    private int _pAlignment;
    private int _pSize = -1;

    public NativeFieldParser(NativeStructMap structs, bool is64Bit)
    {
        _structs = structs;
        _ptrSize = (byte)(is64Bit ? 0x8 : 0x4);
    }

    public NativeFieldParser(NativeStructMap structs, NativeStruct super, bool is64Bit)
        : this(structs, is64Bit)
    {
        _fieldOffset = super.Size;
        _pAlignment = super.Alignment;
    }

    public FieldParseResult ParseNext(string typeName, int? forceAlignment)
    {
        int tBitOffset = _bitOffset, tOffset = _fieldOffset;
        var result = ParseTypeSize(typeName);

        // Result was not a bitfield or we weren't previously in a bitfield.
        // In that case, we align this field to the required alignment.
        if (!result.BitField.HasValue || tBitOffset == -1)
        {
            // If we were previously in a bitfield, but are not anymore, we need to break the byte alignment.
            if (tBitOffset != -1 && _bitOffset == -1 && _pSize == 0)
            {
                _fieldOffset++;
            }

            // We align to either a forced alignment or the maximum between the previous alignment and the current alignment.
            tOffset = Align(_fieldOffset, forceAlignment ?? Math.Max(_pAlignment, result.Alignment));
        }

        _fieldOffset = tOffset + result.Size;
        _pSize = result.Size;
        _pAlignment = result.Alignment;

        return new(typeName, tOffset, result.Size, result.Alignment, result.BitField);
    }

    private (string TypeName, int Size, int Alignment, BitField? BitField) ParseTypeSize(string typeName)
    {
        // If we're not in a bitfield, we simply reset the offset to -1.

        if (IsNativeType(typeName, out int size))
        {
            _bitOffset = -1;
            return (typeName, size, size, null);
        }

        if (IsKnownStruct(typeName, out size, out int alignment))
        {
            _bitOffset = -1;
            return (typeName, size, alignment, null);
        }

        if (_genericRegex.Groups(typeName) is [_, { Value: string genericName }])
        {
            _bitOffset = -1;
            return ParseTypeSize(genericName);
        }

        if (_arrayRegex.Groups(typeName) is [_, { Value: string arrayType }, { Value: string sCount }])
        {
            _bitOffset = -1;

            // We allow empty array sizes to be specified, in which case we default to 1.
            if (!int.TryParse(sCount, out int count))
            {
                count = 1;
            }

            // Get size and alignment for the array's element type recursively.
            (_, size, alignment, _) = ParseTypeSize(arrayType);
            return ($"{arrayType}[]", size * count, alignment, null);
        }

        if (_bitfieldRegex.Groups(typeName) is [_, { Value: string nativeType }, { Value: string sBitFieldSize }])
        {
            // If we're a bitfield, assume it must be of a native type.
            _ = IsNativeType(nativeType, out size);

            // Explicitly let it throw for incorrect inputs.
            int bitFieldSize = int.Parse(sBitFieldSize);

            // If we previously were not in a bitfield, or the underlying type's alignment changed,
            // reset the current bitfield offset to 0.
            if (_bitOffset == -1 || size != _pAlignment)
            {
                _bitOffset = 0;
            }

            // Store the current bitfield offset for creating the bitfield mask.
            int tBitOffset = _bitOffset;

            // Advance bitfield offset by the new size.
            _bitOffset += bitFieldSize;

            alignment = size;
            size = _bitOffset / 8;

            // We only care about the bitfield offset within the current byte.
            _bitOffset %= 8;

            return (typeName, size, alignment, new(bitFieldSize, tBitOffset));
        }

        // If none of the above apply, just assume it's some kind of typedef to a pointer.
        _bitOffset = -1;
        return (typeName, _ptrSize, _ptrSize, null);
    }

    private bool IsKnownStruct(string typeName, out int size, out int alignment)
    {
        // Check the backing data for whether we already know this struct's size and alignment.
        // This is used for in-line (arrays of) structs.
        if (_structs.TryGetValue(typeName, out NativeStruct? es))
        {
            size = es.Size;
            alignment = es.Alignment;

            return true;
        }

        size = alignment = 0;
        return false;
    }

    private bool IsNativeType(string typeName, out int size)
    {
        // C and C++ primitive types are redefined in the JSON files to match C# types.
        // This is to avoid having to accommodate for all combinations of `int`, `long`, `int32_t`, etc.

        size = typeName switch
        {
            "byte" or "sbyte" or "bool" => 0x01,
            "ushort" or "short" or "char" => 0x02,
            "uint" or "int" or "float" => 0x04,
            "ulong" or "long" or "double" => 0x08,
            "decimal" => 0x10,
            "nint" or "nuint" => _ptrSize,
            _ => -1
        };

        return size != -1;
    }

    /// <summary>
    ///     Aligns a given <paramref name="offset"/> to the specified <paramref name="alignment"/>.
    /// </summary>
    /// <param name="offset">The offset to align.</param>
    /// <param name="alignment">The alignment to apply.</param>
    /// <returns>
    ///     The aligned offset.
    /// </returns>
    public static int Align(int offset, int alignment)
    {
        if (alignment <= 0)
        {
            return 0;
        }

        int mod = offset % alignment;
        if (mod > 0)
        {
            offset += alignment - mod;
        }

        return offset;
    }
}
