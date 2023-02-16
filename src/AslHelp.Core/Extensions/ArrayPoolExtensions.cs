using System.Buffers;

namespace AslHelp.Core.Extensions;

internal static class ArrayPoolExtensions
{
    public static T[] Rent<T>(int length)
    {
        return ArrayPool<T>.Shared.Rent(length);
    }

    public static void Return<T>(T[] array)
    {
        if (array is not null)
        {
            ArrayPool<T>.Shared.Return(array);
        }
    }
}
