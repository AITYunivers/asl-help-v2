using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AslHelp.Common.Exceptions;

/// <summary>
///     The <see cref="ThrowHelper"/> class
///     provides helper methods for throwing exceptions.
/// </summary>
public static partial class ThrowHelper
{
    /// <summary>
    ///     Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is <see langword="null"/>.
    /// </summary>
    /// <param name="argument">The reference type argument to validate as non-<see langword="null"/>.</param>
    /// <param name="message">An optional message to include in the exception.</param>
    /// <param name="paramName">
    ///     The name of the parameter with which <paramref name="argument"/> corresponds.
    ///     If this parameter is omitted, the name of <paramref name="argument"/> is used.
    /// </param>
    public static void ThrowIfNull(
        object argument,
        string message = null,
        [CallerArgumentExpression(nameof(argument))] string paramName = null)
    {
        if (argument is null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ThrowArgumentNullException(paramName);
            }
            else
            {
                ThrowArgumentNullException(paramName, message);
            }
        }
    }

    /// <summary>
    ///     Throws an <see cref="ArgumentNullException"/> if <paramref name="collection"/> is <see langword="null"/>.<br/>
    ///     Throws an <see cref="ArgumentException"/> if <paramref name="collection"/> is empty.
    /// </summary>
    /// <param name="collection">The collection argument to validate as non-<see langword="null"/> and non-empty.</param>
    /// <param name="message">An optional message to include in the exception.</param>
    /// <param name="paramName">
    ///     The name of the parameter with which <paramref name="collection"/> corresponds.
    ///     If this parameter is omitted, the name of <paramref name="collection"/> is used.
    /// </param>
    public static void ThrowIfNullOrEmpty<T>(
        IEnumerable<T> collection,
        string message = null,
        [CallerArgumentExpression(nameof(collection))] string paramName = null)
    {
        if (collection is null)
        {
            ThrowArgumentNullException(paramName);
        }

        if (!collection.Any())
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ThrowArgumentException(paramName, "Collection cannot be empty.");
            }
            else
            {
                ThrowArgumentException(paramName, message);
            }
        }
    }

    /// <summary>
    ///     Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="address"/> is less than or equal to zero.
    /// </summary>
    /// <param name="address">The memory address to validate.</param>
    /// <param name="message">An optional message to include in the exception.</param>
    /// <param name="paramName">
    ///     The name of the parameter with which <paramref name="address"/> corresponds.
    ///     If this parameter is omitted, the name of <paramref name="address"/> is used.
    /// </param>
    public static void ThrowIfAddressInvalid(
        nint address,
        string message,
        [CallerArgumentExpression(nameof(address))] string paramName = null)
    {
        if (address <= 0)
        {
            ThrowArgumentOutOfRangeException(paramName, message);
        }
    }

    /// <summary>
    ///     Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than <paramref name="min"/>.
    /// </summary>
    /// <param name="value">The value to be validated.</param>
    /// <param name="min">The minimum value <paramref name="value"/> can be.</param>
    /// <param name="message">An optional message to include in the exception.</param>
    /// <param name="paramName">
    ///     The name of the parameter with which <paramref name="value"/> corresponds.
    ///     If this parameter is omitted, the name of <paramref name="value"/> is used.
    /// </param>
    public static void ThrowIfLessThan<T>(
        T value,
        T min,
        string message = null,
        [CallerArgumentExpression(nameof(value))] string paramName = null)
        where T : unmanaged, IComparable<T>
    {
        if (value.CompareTo(min) < 0)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ThrowArgumentOutOfRangeException(paramName, $"'{paramName}' must be larger than {min}.");
            }
            else
            {
                ThrowArgumentOutOfRangeException(paramName, message);
            }
        }
    }

    /// <summary>
    ///     Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is larger than <paramref name="max"/>.
    /// </summary>
    /// <param name="value">The value to be validated.</param>
    /// <param name="max">The maximum value <paramref name="value"/> can be.</param>
    /// <param name="message">An optional message to include in the exception.</param>
    /// <param name="paramName">
    ///     The name of the parameter with which <paramref name="value"/> corresponds.
    ///     If this parameter is omitted, the name of <paramref name="value"/> is used.
    /// </param>
    public static void ThrowIfLargerThan<T>(
        T value,
        T max,
        string message = null,
        [CallerArgumentExpression(nameof(value))] string paramName = null)
        where T : unmanaged, IComparable<T>
    {
        if (value.CompareTo(max) > 0)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ThrowArgumentOutOfRangeException(paramName, $"'{paramName}' must be less than {max}.");
            }
            else
            {
                ThrowArgumentOutOfRangeException(paramName, message);
            }
        }
    }

    /// <summary>
    ///     Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is
    ///     not in the range of <paramref name="min"/> and <paramref name="max"/>.
    /// </summary>
    /// <param name="value">The value to be validated.</param>
    /// <param name="min">The minimum value <paramref name="value"/> can be.</param>
    /// <param name="max">The maximum value <paramref name="value"/> can be.</param>
    /// <param name="message">An optional message to include in the exception.</param>
    /// <param name="paramName">
    ///     The name of the parameter with which <paramref name="value"/> corresponds.
    ///     If this parameter is omitted, the name of <paramref name="value"/> is used.
    /// </param>
    public static void ThrowIfNotInRange<T>(
        T value,
        T min,
        T max,
        string message = null,
        [CallerArgumentExpression(nameof(value))] string paramName = null)
        where T : unmanaged, IComparable<T>
    {
        ThrowIfLessThan(value, min, message, paramName);
        ThrowIfLargerThan(value, max, message, paramName);
    }
}
