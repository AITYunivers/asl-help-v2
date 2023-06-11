using System;
using System.Buffers;
using AslHelp.Common.Extensions;
using AslHelp.Native.Commands;

namespace AslHelp.Native;

public static partial class AslHelpPipe
{
    private static unsafe PipeResponseCode Deref(out nint result)
    {
        long address = _pipe.Read<long>();
        int length = _pipe.Read<int>();

        int[]? rented = null;
        Span<int> buffer =
            length <= 256
            ? stackalloc int[256]
            : (rented = ArrayPool<int>.Shared.Rent(length));

        Span<int> offsets = buffer[..length];

        _pipe.Read(offsets);

        if (address == 0)
        {
            result = default;
            return PipeResponseCode.NullPtrFailure;
        }

        result = (nint)address;

        if (length == 0)
        {
            return PipeResponseCode.Success;
        }

        foreach (int offset in offsets)
        {
            result = *(nint*)result;

            if (result == 0)
            {
                return PipeResponseCode.NullPtrFailure;
            }

            result += offset;
        }

        ArrayPool<int>.Shared.ReturnIfNotNull(rented);

        return PipeResponseCode.Success;
    }
}
