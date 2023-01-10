namespace AslHelp.Core.Extensions;

internal static class CharExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ToHexValue(this char c)
    {
        int digit = c - '0';

        if (digit < 10)
        {
            return (byte)digit;
        }

        int letter = (c | 0x20) - 'a';

        if (letter < 6)
        {
            return (byte)(letter + 10);
        }

        return 0xFF;
    }
}
