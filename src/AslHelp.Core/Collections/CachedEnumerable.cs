namespace AslHelp.Core.Collections;

/// <summary>
///     The <see cref="CachedEnumerable{TKey, TValue}"/> class
///     provides an abstract interface for enumerable collections with an internal cache.<br/>
///     The cache is populated during enumeration and can be accessed using a key corresponding to the value.
/// </summary>
/// <typeparam name="TKey">The type of the keys for the <see cref="CachedEnumerable{TKey, TValue}"/>.</typeparam>
/// <typeparam name="TValue">The type of the values in the <see cref="CachedEnumerable{TKey, TValue}"/>.</typeparam>
public abstract class CachedEnumerable<TKey, TValue> : IEnumerable<TValue> where TKey : notnull
{
    private readonly IEqualityComparer<TKey> _comparer;
    protected readonly Dictionary<TKey, TValue> _cache;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CachedEnumerable{TKey, TValue}"/> class
    ///     with the default equality comparer for <typeparamref name="TKey"/>.
    /// </summary>
    protected CachedEnumerable()
        : this(EqualityComparer<TKey>.Default) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="CachedEnumerable{TKey, TValue}"/> class
    ///     with the specified equality comparer for <typeparamref name="TKey"/>.
    /// </summary>
    /// <param name="comparer"></param>
    protected CachedEnumerable(IEqualityComparer<TKey> comparer)
    {
        _comparer = comparer;
        _cache = new(comparer);
    }

    /// <summary>
    ///     Gets the number of elements contained in the <see cref="CachedEnumerable{TKey, TValue}"/>'s cache.
    /// </summary>
    public int Count => _cache.Count;

    public abstract IEnumerator<TValue> GetEnumerator();
    protected abstract TKey GetKey(TValue value);

    protected virtual void OnSearch(TKey key) { }
    protected virtual void OnFound(TValue value) { }
    protected virtual void OnNotFound(TKey key) { }

    protected virtual string KeyNotFoundMessage(TKey key)
    {
        return $"The given key '{key}' was not present in the cached collection.";
    }

    public TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out TValue value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException(KeyNotFoundMessage(key));
            }
        }
        protected set => _cache[key] = value;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (_cache.TryGetValue(key, out value))
        {
            return true;
        }

        lock (_cache)
        {
            OnSearch(key);

            foreach (TValue item in this)
            {
                value = item;
                TKey itemKey = GetKey(value);

                _cache[itemKey] = value;

                if (_comparer.Equals(key, itemKey))
                {
                    OnFound(value);

                    return true;
                }
            }
        }

        OnNotFound(key);

        value = default;
        return false;
    }

    public void Clear()
    {
        lock (_cache)
        {
            _cache.Clear();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
