namespace AslHelp.Core.Exceptions;

internal static class ThrowHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNull(object argument, string message = null, [CallerArgumentExpression(nameof(argument))] string paramName = null)
    {
        if (argument is null)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ThrowANE(paramName);
            }
            else
            {
                ThrowANE(paramName, message);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNullOrExited(Process process)
    {
        ThrowIfNull(process);

        if (process.HasExited)
        {
            ThrowIOE("Process has exited.");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfNullOrEmpty<T>(IEnumerable<T> collection, string message = null, [CallerArgumentExpression(nameof(collection))] string paramName = null)
    {
        if (collection is null)
        {
            ThrowANE(paramName);
        }

        if (!collection.Any())
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ThrowAE(paramName, "Collection cannot be empty.");
            }
            else
            {
                ThrowAE(paramName, message);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ThrowIfInvalid(nint address, string message = null, [CallerArgumentExpression(nameof(address))] string paramName = null)
    {
        if (address <= 0)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ThrowAOORE(paramName);
            }
            else
            {
                ThrowAOORE(paramName, message);
            }
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowAE(string paramName, string message)
    {
        throw new ArgumentException(message, paramName);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowANE(string paramName)
    {
        throw new ArgumentNullException(paramName);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowANE(string paramName, string message)
    {
        throw new ArgumentNullException(paramName, message);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowAOORE(string paramName)
    {
        throw new ArgumentOutOfRangeException(paramName);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowAOORE(string paramName, string message)
    {
        throw new ArgumentOutOfRangeException(message, paramName);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowIOE(string message)
    {
        throw new InvalidOperationException(message);
    }
}
