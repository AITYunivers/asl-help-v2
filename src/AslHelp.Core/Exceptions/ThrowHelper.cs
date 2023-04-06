using System.Net;

namespace AslHelp.Core.Exceptions;

internal static partial class ThrowHelper
{
    public static void ThrowIfNull(
        object argument,
        string message = null,
        [CallerArgumentExpression(nameof(argument))] string paramName = null)
    {
        if (argument is null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Throw.ArgumentNull(paramName);
            }
            else
            {
                Throw.ArgumentNull(paramName, message);
            }
        }
    }

    public static void ThrowIfNullOrEmpty<T>(
        IEnumerable<T> collection,
        string message = null,
        [CallerArgumentExpression(nameof(collection))] string paramName = null)
    {
        if (collection is null)
        {
            Throw.ArgumentNull(paramName);
        }

        if (!collection.Any())
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Throw.Argument(paramName, "Collection cannot be empty.");
            }
            else
            {
                Throw.Argument(paramName, message);
            }
        }
    }

    public static void ThrowIfAddressInvalid(
        nint address,
        string message,
        [CallerArgumentExpression(nameof(address))] string paramName = null)
    {
        if (address <= 0)
        {
            Throw.ArgumentOutOfRange(paramName, message);
        }
    }

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
                Throw.ArgumentOutOfRange(paramName, $"'{paramName}' must be larger than {min}.");
            }
            else
            {
                Throw.ArgumentOutOfRange(paramName, message);
            }
        }
    }

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
                Throw.ArgumentOutOfRange(paramName, $"'{paramName}' must be less than {max}.");
            }
            else
            {
                Throw.ArgumentOutOfRange(paramName, message);
            }
        }
    }

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
