using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using AslHelp.Common.Exceptions;

namespace AslHelp.Unity.Collections;

internal sealed partial class NetFx20Dictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    where TKey : unmanaged
    where TValue : unmanaged
{
    public readonly record struct Link(
        int HashCode,
        int Next);

    private readonly int[] _table;
    private readonly Link[] _linkSlots;

    private readonly TKey[] _keySlots;
    private readonly TValue[] _valueSlots;

    public NetFx20Dictionary(int count, int[] table, Link[] linkSlots, TKey[] keySlots, TValue[] valueSlots)
    {
        _table = table;
        _linkSlots = linkSlots;
        _keySlots = keySlots;
        _valueSlots = valueSlots;

        Count = count;
    }

    public int Count { get; }

    public IEnumerable<TKey> Keys
    {
        get
        {
            Enumerator enumerator = new(this);
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current.Key;
            }
        }
    }

    public IEnumerable<TValue> Values
    {
        get
        {
            Enumerator enumerator = new(this);
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current.Value;
            }
        }
    }

    private ref int GetBucket(uint hashCode)
    {
        return ref _table[(hashCode & 0x7FFFFFFF) % _table.Length];
    }

    public TValue this[TKey key]
    {
        get
        {
            ref TValue value = ref FindValue(key);
            if (!Unsafe.IsNullRef(ref value))
            {
                return value;
            }

            string msg = $"The given key '{key}' was not present in the dictionary.";
            ThrowHelper.ThrowKeyNotFoundException(msg);

            return default;
        }
    }

    public bool ContainsKey(TKey key)
    {
        return !Unsafe.IsNullRef(ref FindValue(key));
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        ref TValue valRef = ref FindValue(key);
        if (!Unsafe.IsNullRef(ref valRef))
        {
            value = valRef;
            return true;
        }

        value = default;
        return false;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private ref TValue FindValue(TKey key)
    {
        ref TValue value = ref Unsafe.NullRef<TValue>();

        Link[] linkSlots = _linkSlots;

        uint hashCode = (uint)key.GetHashCode();
        int i = GetBucket(hashCode) - 1;

        while (i >= 0)
        {
            if ((uint)i > (uint)linkSlots.Length)
            {
                break;
            }

            if (linkSlots[i].HashCode == hashCode && EqualityComparer<TKey>.Default.Equals(_keySlots[i], key))
            {
                value = ref _valueSlots[i];
                break;
            }

            i = linkSlots[i].Next;
        }

        return ref value;
    }
}
