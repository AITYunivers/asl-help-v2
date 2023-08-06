using System.Collections.Generic;

namespace AslHelp.Mono.Collections;

public readonly record struct MonoKeyValuePair<TKey, TValue>
    where TKey : unmanaged
    where TValue : unmanaged
{
#pragma warning disable CS0169, IDE0051
    private readonly ulong _padding;
#pragma warning restore CS0169, IDE0051

    public MonoKeyValuePair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }

    public TKey Key { get; }
    public TValue Value { get; }

    public static implicit operator KeyValuePair<TKey, TValue>(MonoKeyValuePair<TKey, TValue> pair)
    {
        return new(pair.Key, pair.Value);
    }

    public static implicit operator MonoKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> pair)
    {
        return new(pair.Key, pair.Value);
    }

    public override string ToString()
    {
        return $"[{Key}, {Value}]";
    }
}
