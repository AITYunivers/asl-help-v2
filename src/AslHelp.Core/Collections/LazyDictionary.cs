using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.Collections;

/// <summary>
///     The <see cref="LazyDictionary{TKey, TValue}"/> class
///     provides an abstract interface for enumerable collections with an internal cache.<br/>
///     The cache is populated during enumeration and can be accessed using a key corresponding to the value.
/// </summary>
/// <typeparam name="TKey">The type of the keys for the <see cref="LazyDictionary{TKey, TValue}"/>.</typeparam>
/// <typeparam name="TValue">The type of the values in the <see cref="LazyDictionary{TKey, TValue}"/>.</typeparam>
public abstract class LazyDictionary<TKey, TValue> : IEnumerable<TValue>
    where TKey : notnull
{
    private readonly IEqualityComparer<TKey> _comparer;

    /// <summary>
    ///     The internal cache of the <see cref="LazyDictionary{TKey, TValue}"/>.
    /// </summary>
    protected readonly Dictionary<TKey, TValue?> _cache;

    /// <summary>
    ///     Initializes a new instance of the <see cref="LazyDictionary{TKey, TValue}"/> class
    ///     with the default equality comparer for <typeparamref name="TKey"/>.
    /// </summary>
    protected LazyDictionary()
        : this(EqualityComparer<TKey>.Default) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="LazyDictionary{TKey, TValue}"/> class
    ///     with the specified equality comparer for <typeparamref name="TKey"/>.
    /// </summary>
    /// <param name="comparer"></param>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="comparer"/> is <see langword="null"/>.
    /// </exception>
    protected LazyDictionary(IEqualityComparer<TKey> comparer)
    {
        ThrowHelper.ThrowIfNull(comparer);

        _comparer = comparer;
        _cache = new(comparer);
    }

    /// <summary>
    ///     Gets the number of elements contained in the <see cref="LazyDictionary{TKey, TValue}"/>'s cache.
    /// </summary>
    public int Count => _cache.Count;

    /// <summary>
    ///     Returns an enumerator that iterates through the <see cref="LazyDictionary{TKey, TValue}"/>.
    /// </summary>
    public abstract IEnumerator<TValue> GetEnumerator();

    /// <summary>
    ///     Gets a unique identifier for the given <paramref name="value"/>.
    /// </summary>
    /// <param name="value">The value whose unique identifier is to be gotten.</param>
    /// <returns>
    ///     A key that is entirely unique to the given <paramref name="value"/>.
    /// </returns>
    protected abstract TKey GetKey(TValue value);

    /// <summary>
    ///     Called when <see cref="TryGetValue(TKey, out TValue)"/> begins to search for a value
    ///     with the given <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The <typeparamref name="TKey"/> to search for.</param>
    protected virtual void OnSearch(TKey key) { }

    /// <summary>
    ///     Called when <see cref="TryGetValue(TKey, out TValue)"/> finds a value
    ///     with the given <paramref name="key"/>.
    /// </summary>
    /// <param name="value">The found <typeparamref name="TValue"/>.</param>
    protected virtual void OnFound(TValue value) { }

    /// <summary>
    ///     Called when <see cref="TryGetValue(TKey, out TValue)"/> does not find a value
    ///     with the given <paramref name="key"/>. Only occurs after enumerating the entire collection.
    /// </summary>
    /// <param name="key">The <typeparamref name="TKey"/> that was searched for.</param>
    protected virtual void OnNotFound(TKey key) { }

    /// <summary>
    ///     Retrieves a custom message for when <see cref="this[TKey]"/> is called
    ///     with a key that is not present in the <see cref="LazyDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <param name="key">The <typeparamref name="TKey"/> that was searched for.</param>
    protected virtual string KeyNotFoundMessage(TKey key)
    {
        return $"The given key '{key}' was not present in the cached collection.";
    }

    /// <summary>
    ///     Gets or sets the <typeparamref name="TValue"/> associated with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key whose value is to be gotten or set.</param>
    /// <returns>
    ///     The <typeparamref name="TValue"/> associated with the specified <paramref name="key"/>, if it exists.
    /// </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown when no value corresponding to the given <paramref name="key"/>
    ///     is present in the <see cref="LazyDictionary{TKey, TValue}"/>.
    /// </exception>
    public TValue? this[TKey key]
    {
        get
        {
            if (!TryGetValue(key, out TValue? value))
            {
                string msg = KeyNotFoundMessage(key);
                ThrowHelper.ThrowKeyNotFoundException(msg);
            }

            return value;
        }
        protected set => _cache[key] = value;
    }

    /// <summary>
    ///     Gets the value associated with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">The key of the value to get.</param>
    /// <param name="value">
    ///     The value associated with the specified <paramref name="key"/> if the method return <see langword="true"/>;
    ///     otherwise, <see langword="default"/>(<typeparamref name="TValue"/>).
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the <see cref="LazyDictionary{TKey, TValue}"/> contains an element
    ///     with the specified <paramref name="key"/>;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetValue(TKey key, [MaybeNullWhen(true)] out TValue? value)
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

    /// <summary>
    ///     Clears the cache.
    /// </summary>
    public void Clear()
    {
        lock (_cache)
        {
            _cache.Clear();
        }
    }

    /// <summary>
    ///     Returns an enumerator that iterates through the <see cref="LazyDictionary{TKey, TValue}"/>.
    /// </summary>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
