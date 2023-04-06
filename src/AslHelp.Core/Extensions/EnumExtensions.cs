using LiveSplit.ComponentUtil;
using System.Text;

namespace AslHelp.Core.Extensions;

internal static class EnumExtensions
{
    private static readonly (bool, byte, Encoding) _asciiInfo = (false, 1, Encoding.ASCII);
    private static readonly (bool, byte, Encoding) _utf8Info = (false, 1, Encoding.UTF8);
    private static readonly (bool, byte, Encoding) _unicodeInfo = (true, 2, Encoding.Unicode);

    public static (bool, byte, Encoding) GetEncodingInformation(this ReadStringType stringType)
    {
        return stringType switch
        {
            ReadStringType.ASCII => _asciiInfo,
            ReadStringType.UTF8 => _utf8Info,
            ReadStringType.UTF16 => _unicodeInfo,
            _ => throw new ArgumentException($"Provided {nameof(ReadStringType)} was unsupported.", nameof(stringType))
        };
    }
}
