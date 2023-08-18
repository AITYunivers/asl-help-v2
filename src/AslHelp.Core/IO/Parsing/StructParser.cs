using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
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
        [property: JsonPropertyName("alignment")] int? Alignment);

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

            NativeFieldParser parser =
                s.Super is not null && nsm.TryGetValue(s.Super, out var super)
                ? new(nsm, super, is64Bit)
                : new(nsm, is64Bit);

            foreach (Field f in s.Fields)
            {
                var result = parser.ParseNext(f.Type, f.Alignment);
                ns[f.Name] = new(result.BitField)
                {
                    Name = f.Name,
                    Type = result.TypeName,
                    Offset = result.Offset,
                    Size = result.Size,
                    Alignment = result.Alignment
                };
            }

            nsm[s.Name] = ns;
        }

        return nsm;
    }

    public override string ToString()
    {
        StringBuilder sb = new();

        foreach (NativeStruct ns in this)
        {
            if (ns.Super is string super)
            {
                sb.AppendLine($"{ns.Name} : {super} // 0x{ns.Size:X3} (0x{ns.SelfAlignedSize:X3})");
            }
            else
            {
                sb.AppendLine($"{ns.Name} // 0x{ns.Size:X3} (0x{ns.SelfAlignedSize:X3})");
            }

            foreach (NativeField nf in ns.Fields)
            {
                if (nf.BitMask > 0)
                {
                    sb.AppendLine($"    {nf.Type,-32} {nf.Name,-32} " +
                                  $"// 0x{nf.Offset:X3} " +
                                  $"(0x{nf.Size:X3}, 0b{Convert.ToString(nf.BitMask, 2).PadLeft(8, '0')})");
                }
                else
                {
                    sb.AppendLine($"    {nf.Type,-32} {nf.Name,-32} // 0x{nf.Offset:X3} (0x{nf.Size:X3})");
                }
            }
        }

        return sb.ToString();
    }
}

public abstract class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IList<TValue>, IReadOnlyList<TValue>
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dict;
    private readonly Dictionary<TKey, int> _indices;
    private readonly List<TValue> _items = new();

    protected OrderedDictionary(IEqualityComparer<TKey>? comparer = null)
    {
        comparer ??= EqualityComparer<TKey>.Default;

        _dict = new(comparer);
        _indices = new(comparer);
    }

    public ICollection<TKey> Keys => _dict.Keys;
    public ICollection<TValue> Values => _items;

    public int Count => _items.Count;
    public bool IsReadOnly => false;

    public TValue this[TKey key]
    {
        get => _dict[key];
        set
        {
            _dict[key] = value;

            if (_indices.TryGetValue(key, out int index))
            {
                _items.RemoveAt(index);
                _items.Insert(index, value);
            }
            else
            {
                _indices[key] = _items.Count;
                _items.Add(value);
            }
        }
    }

    public TValue this[int index]
    {
        get => _items[index];
        set
        {
            _items[index] = value;

            TKey key = GetKeyForItem(value);
            _dict[key] = value;
            _indices[key] = index;
        }
    }

    protected abstract TKey GetKeyForItem(TValue item);

    public void Add(TValue item)
    {
        Add(GetKeyForItem(item), item);
    }

    public void Add(TKey key, TValue value)
    {
        _dict.Add(key, value);
        _indices.Add(key, _items.Count);
        _items.Add(value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Insert(int index, TValue item)
    {
        foreach (var kvp in _indices.Where(kvp => kvp.Value >= index))
        {
            _indices[kvp.Key] = kvp.Value + 1;
        }

        TKey key = GetKeyForItem(item);

        _dict.Add(key, item);
        _indices.Add(key, index);
        _items.Insert(index, item);
    }

    public bool Remove(TValue item)
    {
        return Remove(GetKeyForItem(item));
    }

    public bool Remove(TKey key)
    {
        if (!_indices.TryGetValue(key, out int index))
        {
            return false;
        }

        foreach (var kvp in _indices.Where(kvp => kvp.Value > index))
        {
            _indices[kvp.Key] = kvp.Value - 1;
        }

        _items.RemoveAt(index);
        return _dict.Remove(key) && _indices.Remove(key);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }

    public void RemoveAt(int index)
    {
        TKey key = GetKeyForItem(_items[index]);

        foreach (var kvp in _indices.Where(kvp => kvp.Value > index))
        {
            _indices[kvp.Key] = kvp.Value - 1;
        }

        _dict.Remove(key);
        _indices.Remove(key);
        _items.RemoveAt(index);
    }

    public bool ContainsKey(TKey key)
    {
        return _dict.ContainsKey(key);
    }

    public bool Contains(TValue item)
    {
        return Contains(new KeyValuePair<TKey, TValue>(GetKeyForItem(item), item));
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _dict.Contains(item);
    }

    public int IndexOf(TValue item)
    {
        return _indices[GetKeyForItem(item)];
    }

    public bool TryGetValue(TKey key, [UnscopedRef] out TValue value)
    {
        return _dict.TryGetValue(key, out value);
    }

    public void Clear()
    {
        _dict.Clear();
        _indices.Clear();
        _items.Clear();
    }

    public void CopyTo(TValue[] array, int arrayIndex)
    {
        if (arrayIndex < 0)
        {
            const string msg = "Starting index must be equal to or greater than 0.";
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(arrayIndex), msg);
        }

        for (int i = 0; i < Count; i++)
        {
            if (arrayIndex >= array.Length)
            {
                const string msg = $"The number of elements in this collection is greater than the available space in '{nameof(array)}'.";
                ThrowHelper.ThrowArgumentException(nameof(array), msg);
            }

            array[arrayIndex++] = _items[i];
        }
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        if (arrayIndex < 0)
        {
            const string msg = "Starting index must be equal to or greater than 0.";
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(arrayIndex), msg);
        }

        for (int i = 0; i < Count; i++)
        {
            if (arrayIndex >= array.Length)
            {
                const string msg = $"The number of elements in this collection is greater than the available space in '{nameof(array)}'.";
                ThrowHelper.ThrowArgumentException(nameof(array), msg);
            }

            TValue item = _items[i];
            array[arrayIndex++] = new(GetKeyForItem(item), item);
        }
    }

    public IEnumerator<TValue> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
        return _dict.GetEnumerator();
    }
}

public sealed partial class NativeStructMap : OrderedDictionary<string, NativeStruct>
{
    protected override string GetKeyForItem(NativeStruct item)
    {
        return item.Name;
    }
}

public sealed class NativeStruct : OrderedDictionary<string, NativeField>
{
    public IEnumerable<NativeField> Fields => this;

    public required string Name { get; init; }
    public required string? Super { get; init; }

    public int Size => Fields.First().Offset;
    public int Alignment => Fields.Last().Alignment;
    public int SelfAlignedSize => NativeFieldParser.Align(Size, Alignment);

    protected override string GetKeyForItem(NativeField item)
    {
        return item.Name;
    }
}

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
