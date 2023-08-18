using System;

namespace AslHelp.Core.IO.Parsing;

public record struct FieldParseResult(
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
    private readonly NativeStructMap _structs;
    private readonly byte _ptrSize;

    private int _offset;
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
        _offset = super.Size;
        _pAlignment = super.Alignment;
    }

    public FieldParseResult ParseNext(string typeName, int? forceAlignment)
    {
        int tBitOffset = _bitOffset, tOffset = _offset;
        SizeParseResult result = ParseTypeSize(typeName);

        if (!result.BitField.HasValue || tBitOffset == -1)
        {
            if (tBitOffset != -1 && _bitOffset == -1 && _pSize == 0)
            {
                _offset++;
            }

            tOffset = Align(_offset, forceAlignment ?? Math.Max(_pAlignment, result.Alignment));
        }

        _offset = tOffset + result.Size;
        _pSize = result.Size;
        _pAlignment = result.Alignment;

        return new(typeName, tOffset, result.Size, result.Alignment, result.BitField);
    }

    private record struct SizeParseResult(
        string TypeName,
        int Size,
        int Alignment,
        BitField? BitField);

    private SizeParseResult ParseTypeSize(string typeName)
    {
        if (IsNativeType(typeName, out int size))
        {
            _bitOffset = -1;
            return new(typeName, size, size, null);
        }

        if (IsKnownStruct(typeName, out size, out int alignment))
        {
            _bitOffset = -1;
            return new(typeName, size, alignment, null);
        }

        if (typeName is [.. string genericName, '<', .., '>'])
        {
            _bitOffset = -1;
            return ParseTypeSize(genericName);
        }

        if (typeName is [.. string arrayType, '[', .. string sCount, ']'])
        {
            _bitOffset = -1;

            if (!int.TryParse(sCount, out int count))
            {
                count = 1;
            }

            (_, size, alignment, _) = ParseTypeSize(arrayType);
            return new($"{arrayType}[]", size * count, alignment, null);
        }

        if (typeName is [.. string nativeType, ':', .. string sBitFieldSize])
        {
            _ = IsNativeType(nativeType, out size);
            int bitFieldSize = int.Parse(sBitFieldSize);

            // If we previously were not in a bitfield, or the type's alignment changed,
            // reset the current bit-offset to 0.
            if (_bitOffset == -1 || size != _pAlignment)
            {
                _bitOffset = 0;
            }

            // Advance bit-offset by the new size.
            int tBitOffset = _bitOffset;
            _bitOffset += bitFieldSize;

            alignment = size;
            size = _bitOffset / 8;

            _bitOffset %= 8;

            return new(typeName, size, alignment, new(bitFieldSize, tBitOffset));
        }

        _bitOffset = -1;
        return new(typeName, _ptrSize, _ptrSize, null);
    }

    private bool IsKnownStruct(string typeName, out int size, out int alignment)
    {
        if (_structs.TryGetValue(typeName, out NativeStruct? es))
        {
            size = es.Size;
            alignment = es.Alignment;

            return true;
        }

        size = alignment = _ptrSize;
        return false;
    }

    private bool IsNativeType(string typeName, out int size)
    {
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
