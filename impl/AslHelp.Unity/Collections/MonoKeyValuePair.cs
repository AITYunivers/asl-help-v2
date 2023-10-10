using System.Collections.Generic;

namespace AslHelp.Unity.Collections;

public readonly struct DictionaryEntry<TKey, TValue>
    where TKey : unmanaged
    where TValue : unmanaged
{
    public DictionaryEntry(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }

    public int HashCode { get; }
    public int Next { get; }

    public TKey Key { get; }
    public TValue Value { get; }

    public static implicit operator KeyValuePair<TKey, TValue>(DictionaryEntry<TKey, TValue> pair)
    {
        return new(pair.Key, pair.Value);
    }

    public static implicit operator DictionaryEntry<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
    {
        return new(pair.Key, pair.Value);
    }

    public override string ToString()
    {
        return $"[{Key}, {Value}]";
    }
}
