using AslHelp.Core.Exceptions;

namespace AslHelp.Core.Extensions;

internal static class StringExtensions
{
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

    public static unsafe string Concat(this string[] source)
    {
        ThrowHelper.ThrowIfNullOrEmpty(source);

        int length = 0;
        foreach (string s in source)
        {
            length += s.Length;
        }

        ReadOnlySpan<string> strings = source;
        Span<char> buffer = stackalloc char[length];

        for (int i = 0, offset = 0; i < strings.Length; i++)
        {
            strings[i].AsSpan().CopyTo(buffer[offset..]);
            offset += strings[i].Length;
        }

        return buffer.ToString();
    }
}
