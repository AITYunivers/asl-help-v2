using System;
using AslHelp.Common.Exceptions;

namespace AslHelp.Common.Extensions;

/// <summary>
///     The <see cref="StringExtensions"/> class
///     provides useful extension methods for the <see cref="string"/> type.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Provides an optimized method for removing all whitespace from a string.
    /// </summary>
    /// <param name="value">The string to remove the whitespace characters from.</param>
    /// <returns>
    ///     The string with all whitespace removed.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static unsafe string RemoveWhiteSpace(this string value)
    {
        ThrowHelper.ThrowIfNull(value);

        char* output = stackalloc char[value.Length];

        fixed (char* pValue = value)
        {
            for (int i = 0, j = 0; i < value.Length; i++)
            {
                char c = pValue[i];

                if (!char.IsWhiteSpace(c))
                {
                    output[j++] = c;
                }
            }

            return new(output);
        }
    }

    /// <summary>
    ///     Provides an optimized method to concatenate the elements of a specified <see cref="string"/> array.
    /// </summary>
    /// <param name="values">The collection of strings to concatenate.</param>
    /// <returns>
    ///     The concatenated elements of <paramref name="values"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="values"/> is <see langword="null"/>.
    /// </exception>
    public static string Concat(this string[] values)
    {
        ThrowHelper.ThrowIfNull(values);

        if (values.Length == 0)
        {
            return "";
        }

        int length = 0;
        foreach (string s in values)
        {
            length += s.Length;
        }

        ReadOnlySpan<string> strings = values;
        Span<char> buffer = stackalloc char[length];

        for (int i = 0, offset = 0; i < strings.Length; i++)
        {
            strings[i].AsSpan().CopyTo(buffer[offset..]);
            offset += strings[i].Length;
        }

        return buffer.ToString();
    }
}
