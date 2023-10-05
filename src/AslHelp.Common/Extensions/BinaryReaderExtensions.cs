using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AslHelp.Common.Extensions;

public static class BinaryReaderExtensions
{
    public static string ReadNullTerminatedString(this BinaryReader reader)
    {
        List<byte> bytes = [];

        while (reader.ReadByte() is byte b && b != 0)
        {
            bytes.Add(b);
        }

        return Encoding.UTF8.GetString(bytes.ToArray());
    }
}
