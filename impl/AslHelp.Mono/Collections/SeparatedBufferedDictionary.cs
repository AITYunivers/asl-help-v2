using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using AslHelp.Common.Exceptions;
using AslHelp.Mono.Memory.Ipc;

namespace AslHelp.Mono.Collections;

public class SeparatedBufferedDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    where TKey : unmanaged
    where TValue : unmanaged
{
    private readonly TKey[] _keys;
    private readonly TValue[] _values;

    public SeparatedBufferedDictionary(TKey[] keys, TValue[] values)
    {
        if (keys.Length != values.Length)
        {
            string msg = $"The length of {nameof(keys)} and {nameof(values)} must be equal.";
            throw new ArgumentException(msg);
        }

        _keys = keys;
        _values = values;

        Count = keys.Length;
    }

    public int Count { get; }
    public IEnumerable<TKey> Keys => _keys;
    public IEnumerable<TValue> Values => _values;

    public TValue this[TKey key]
    {
        get
        {
            int i = Array.BinarySearch(_keys, key);
            if (i == -1)
            {
                string msg = $"The given key '{key}' was not present in the dictionary.";
                ThrowHelper.ThrowKeyNotFoundException(msg);
            }

            return _values[i];
        }
    }

    public bool ContainsKey(TKey key)
    {
        int i = Array.BinarySearch(_keys, key);
        return i != -1;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        int i = Array.BinarySearch(_keys, key);
        if (i == -1)
        {
            value = default;
            return false;
        }
        else
        {
            value = _values[i];
            return true;
        }
    }

    [SkipLocalsInit]
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return new(_keys[i], _values[i]);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public sealed class SeparatedBufferedDictionary<TValue> : IReadOnlyDictionary<string, TValue>
    where TValue : unmanaged
{
    private readonly Dictionary<string, TValue> _cache;

    private readonly IMonoMemoryReader _memory;

    private readonly nuint[] _keys;
    private readonly TValue[] _values;

    public SeparatedBufferedDictionary(IMonoMemoryReader memory, nuint[] keys, TValue[] values)
    {
        _memory = memory;
        _keys = keys;
        _values = values;

        _cache = new Dictionary<string, TValue>(keys.Length);
    }

    public TValue this[string key] => throw new NotImplementedException();

    public IEnumerable<string> Keys => throw new NotImplementedException();

    public IEnumerable<TValue> Values => throw new NotImplementedException();

    public int Count => throw new NotImplementedException();

    public bool ContainsKey(string key)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public bool TryGetValue(string key, out TValue value)
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
