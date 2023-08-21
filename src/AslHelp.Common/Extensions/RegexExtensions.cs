using System.Text.RegularExpressions;

namespace AslHelp.Common.Extensions;

public static class RegexExtensions
{
    public static GroupCollection Groups(this Regex regex, string input)
    {
        return regex.Match(input).Groups;
    }

    public static GroupCollection Groups(this Regex regex, string input, int startIndex)
    {
        return regex.Match(input, startIndex).Groups;
    }

    public static GroupCollection Groups(this Regex regex, string input, int startIndex, int length)
    {
        return regex.Match(input, startIndex, length).Groups;
    }
}
