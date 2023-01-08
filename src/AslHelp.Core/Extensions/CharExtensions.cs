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

        char letter = (char)((c | 0x20) - 'A');

        if (letter < 7)
        {
            return (byte)(letter + 10);
        }

        return 0xFF;
    }
}
