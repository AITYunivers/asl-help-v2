namespace AslHelp.Core.Exceptions;

internal static partial class ThrowHelper
{
    public static void ThrowIfNull(object argument, string message = null, [CallerArgumentExpression(nameof(argument))] string paramName = null)
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

    public static void ThrowIfNullOrEmpty<T>(IEnumerable<T> collection, string message = null, [CallerArgumentExpression(nameof(collection))] string paramName = null)
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

    public static void ThrowIfAddressInvalid(nint address, string message, [CallerArgumentExpression(nameof(address))] string paramName = null)
    {
        if (address <= 0)
        {
            Throw.ArgumentOutOfRange(paramName, message);
        }
    }
}
