using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using AslHelp.Common.Exceptions;

namespace AslHelp.Common.Extensions;

public static class StreamExtensions
{
    public static unsafe T Read<T>(this Stream stream)
        where T : unmanaged
    {
        T value;
        stream.ReadExactly(new(&value, sizeof(T)));

        return value;
    }

    public static unsafe bool TryRead<T>(this Stream stream, out T value)
        where T : unmanaged
    {
        T lValue;
        if (stream.ReadAtLeast(new(&lValue, sizeof(T)), sizeof(T)) == sizeof(T))
        {
            value = lValue;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }

    public static void Read<T>(this Stream stream, Span<T> buffer)
        where T : unmanaged
    {
        stream.ReadExactly(MemoryMarshal.AsBytes(buffer));
    }

    public static unsafe bool TryRead<T>(this Stream stream, Span<T> buffer)
        where T : unmanaged
    {
        int minimumBytes = buffer.Length * sizeof(T);
        if (stream.ReadAtLeast(MemoryMarshal.AsBytes(buffer), minimumBytes) == minimumBytes)
        {
            return true;
        }
        else
        {
            buffer.Clear();
            return false;
        }
    }

    // Polyfilled from CommunityToolkit.HighPerformance.StreamExtensions.

    /// <summary>
    ///     Writes a value of a specified type into a target <see cref="Stream"/> instance.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of value to write.
    /// </typeparam>
    /// <param name="stream">
    ///     The target <see cref="Stream"/> instance to write to.
    /// </param>
    /// <param name="value">
    ///     The input value to write to <paramref name="stream"/>.
    /// </param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe void Write<T>(this Stream stream, in T value)
        where T : unmanaged
    {
        int length = sizeof(T);
        byte[] buffer = ArrayPool<byte>.Shared.Rent(length);

        try
        {
            Unsafe.WriteUnaligned(ref buffer[0], value);

            stream.Write(buffer, 0, length);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private static void ReadExactly(this Stream stream, Span<byte> buffer)
    {
        ThrowHelper.ThrowIfNull(stream);

        _ = ReadAtLeastCore(stream, buffer, buffer.Length, throwOnEndOfStream: true);
    }

    private static int ReadAtLeast(this Stream stream, Span<byte> buffer, int minimumBytes, bool throwOnEndOfStream = true)
    {
        ThrowHelper.ThrowIfNull(stream);

        ValidateReadAtLeastArguments(buffer.Length, minimumBytes);
        return ReadAtLeastCore(stream, buffer, minimumBytes, throwOnEndOfStream);
    }

    private static int ReadAtLeastCore(Stream stream, Span<byte> buffer, int minimumBytes, bool throwOnEndOfStream)
    {
        int totalRead = 0;

        while (totalRead < minimumBytes)
        {
            int read = CommunityToolkit.HighPerformance.StreamExtensions.Read(stream, buffer[totalRead..]);

            if (read == 0)
            {
                if (throwOnEndOfStream)
                {
                    ThrowEndOfFileException();
                }

                return totalRead;
            }

            totalRead += read;
        }

        return totalRead;
    }

    private static void ValidateReadAtLeastArguments(int bufferLength, int minimumBytes)
    {
        if (minimumBytes < 0)
        {
            string msg = "Non-negative number required.";
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(minimumBytes), msg);
        }

        if (bufferLength < minimumBytes)
        {
            string msg = "Offset and length were out of bounds for the array " +
                "or count is greater than the number of elements from index to the end of the source collection.";
            ThrowHelper.ThrowArgumentOutOfRangeException(nameof(minimumBytes), msg);
        }
    }

    private static void ThrowEndOfFileException()
    {
        throw new EndOfStreamException();
    }
}
