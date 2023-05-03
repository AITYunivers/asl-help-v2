using System.Buffers;

namespace AslHelp.Core.Extensions;

/// <summary>
///     The <see cref="ArrayPoolExtensions"/> class
///     provides useful extension methods for the <see cref="ArrayPool{T}"/> class.
/// </summary>
internal static class ArrayPoolExtensions
{
    /// <summary>
    ///     Retrieves a buffer that is at least the requested length.
    /// </summary>
    /// <typeparam name="T">The type of the objects that are in the resource pool.</typeparam>
    /// <param name="minimumLength">The minimum length of the array.</param>
    /// <returns>
    ///     An array of type <typeparamref name="T"/>[] that is at least <paramref name="minimumLength"/> in length.
    /// </returns>
    public static T[] Rent<T>(int minimumLength)
    {
        return ArrayPool<T>.Shared.Rent(minimumLength);
    }

    /// <summary>
    ///     If <paramref name="array"/> is not <see langword="null"/>,
    ///     returns an array to the pool that was previously obtained using the <see cref="ArrayPool{T}.Rent(int)"/>
    ///     method on the same <see cref="ArrayPool{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">The type of the objects that are in the resource pool.</typeparam>
    /// <param name="array">The buffer to return to the pool.</param>
    public static void Return<T>(T[] array)
    {
        if (array is not null)
        {
            ArrayPool<T>.Shared.Return(array);
        }
    }
}
