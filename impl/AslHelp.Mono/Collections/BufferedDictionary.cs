using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using AslHelp.Common.Exceptions;

namespace AslHelp.Mono.Collections;

public sealed class BufferedDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    where TKey : unmanaged
    where TValue : unmanaged
{
    private readonly Dictionary<TKey, TValue> _cache;

    private readonly MonoKeyValuePair<TKey, TValue>[]? _entries;

    private readonly TKey[]? _keys;
    private readonly TValue[]? _values;

    private int _index;

    public BufferedDictionary(MonoKeyValuePair<TKey, TValue>[] entries)
    {
        int count = entries.Length;

        _entries = entries;
        _cache = new(count);

        Count = count;
        UsesEntries = true;
    }

    public BufferedDictionary(TKey[] keys, TValue[] values)
    {
        if (keys.Length != values.Length)
        {
            string msg = $"Keys and values must be of the same length.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        int count = keys.Length;

        _keys = keys;
        _values = values;
        _cache = new(count);

        Count = count;
    }

    [MemberNotNullWhen(true, nameof(_entries))]
    [MemberNotNullWhen(false, nameof(_keys), nameof(_values))]
    private bool UsesEntries { get; }

    public int Count { get; }

    public IEnumerable<TKey> Keys => UsesEntries ? _entries.Select(e => e.Key) : _keys;
    public IEnumerable<TValue> Values => UsesEntries ? _entries.Select(e => e.Value) : _values;

    public TValue this[TKey key] => throw new System.NotImplementedException();

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (_cache.TryGetValue(key, out value))
        {
            return true;
        }

        foreach (var entry in this)
        {
            if (entry.Key.Equals(key))
            {
                value = entry.Value;
                return true;
            }
        }
    }

    public bool ContainsKey(TKey key)
    {
        if (_cache.ContainsKey(key))
        {
            return true;
        }

        foreach (var entry in this)
        {
            if (entry.Key.Equals(key))
            {
                return true;
            }
        }

        return false;
    }

    [SkipLocalsInit]
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (var entry in _cache)
        {
            yield return entry;
        }

        for (; _index < Count; _index++)
        {
            if (UsesEntries)
            {
                var entry = _entries[_index];
                _cache.Add(entry.Key, entry.Value);

                yield return entry;
            }
            else
            {
                var (key, value) = (_keys[_index], _values[_index]);
                _cache.Add(key, value);

                yield return new(key, value);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
