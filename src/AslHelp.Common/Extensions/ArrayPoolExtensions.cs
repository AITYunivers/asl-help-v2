using System.Buffers;

namespace AslHelp.Common.Extensions;

/// <summary>
///     The <see cref="ArrayPoolExtensions"/> class
///     provides useful extension methods for the <see cref="ArrayPool{T}"/> type.
/// </summary>
public static class ArrayPoolExtensions
{
    /// <summary>
    ///     If <paramref name="array"/> is not <see langword="null"/>,
    ///     returns it to the pool that it was previously obtained from using the <see cref="ArrayPool{T}.Rent(int)"/>
    ///     method on the same <see cref="ArrayPool{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">The type of the objects that are in the resource pool.</typeparam>
    /// <param name="array">The buffer to return to the pool.</param>
    public static void ReturnIfNotNull<T>(this ArrayPool<T> pool, T[]? array)
    {
        if (array is not null)
        {
            pool.Return(array);
        }
    }
}
