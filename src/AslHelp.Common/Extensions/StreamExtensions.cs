using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using AslHelp.Common.Exceptions;

namespace AslHelp.Common.Extensions;

/// <summary>
///     The <see cref="StreamExtensions"/> class
///     provides extension methods for the <see cref="Stream"/> class.
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    ///     Reads a value of a type <typeparamref name="T"/> from a <see cref="Stream"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to read.</typeparam>
    /// <param name="stream">The <see cref="Stream"/> to read from.</param>
    /// <returns>The value read from the <see cref="Stream"/>.</returns>
    public static unsafe T Read<T>(this Stream stream)
        where T : unmanaged
    {
        T value;
        stream.ReadExactly(new(&value, sizeof(T)));

        return value;
    }

    /// <summary>
    ///     Attempts to read a value of a type <typeparamref name="T"/> from a <see cref="Stream"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to read.</typeparam>
    /// <param name="stream">The <see cref="Stream"/> to read from.</param>
    /// <param name="value">
    ///     Contains the value read from the <see cref="Stream"/> if the operation succeeded;
    ///     otherwise, <see langword="default"/>(<typeparamref name="T"/>).
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the operation succeeded;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
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

    /// <summary>
    ///     Reads a sequence of values of type <typeparamref name="T"/> from a <see cref="Stream"/>
    ///     into a <see cref="Span{T}"/> buffer.
    /// </summary>
    /// <typeparam name="T">The type of value to read.</typeparam>
    /// <param name="stream">The <see cref="Stream"/> to read from.</param>
    /// <param name="buffer">The <see cref="Span{T}"/> that will be filled with the read values.</param>
    public static void Read<T>(this Stream stream, Span<T> buffer)
        where T : unmanaged
    {
        stream.ReadExactly(MemoryMarshal.AsBytes(buffer));
    }

    /// <summary>
    ///     Attempts to read a sequence of values of a type <typeparamref name="T"/> from a <see cref="Stream"/>
    ///     into a <see cref="Span{T}"/> buffer.
    /// </summary>
    /// <typeparam name="T">The type of value to read.</typeparam>
    /// <param name="stream">The <see cref="Stream"/> to read from.</param>
    /// <param name="buffer">The <see cref="Span{T}"/> that will be filled with the read values.</param>
    /// <returns>
    ///     <see langword="true"/> if the operation succeeded;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static unsafe bool TryRead<T>(this Stream stream, Span<T> buffer)
        where T : unmanaged
    {
        int minimumBytes = buffer.Length * sizeof(T);
        return stream.ReadAtLeast(MemoryMarshal.AsBytes(buffer), minimumBytes) == minimumBytes;
    }

    /// <summary>
    ///     Writes a value of type <typeparamref name="T"/> to a <see cref="Stream"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to write.</typeparam>
    /// <param name="stream">The <see cref="Stream"/> to write to.</param>
    /// <param name="value">The value to write to the <see cref="Stream"/>.</param>
    public static unsafe void Write<T>(this Stream stream, in T value)
        where T : unmanaged
    {
        int length = sizeof(T);
        byte[] buffer = ArrayPool<byte>.Shared.Rent(length);

        Unsafe.WriteUnaligned(ref buffer[0], value);
        stream.Write(buffer, 0, length);

        ArrayPool<byte>.Shared.Return(buffer);
    }

    /// <summary>
    ///     Writes a value of type <typeparamref name="T"/> to a <see cref="Stream"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to write.</typeparam>
    /// <param name="stream">The <see cref="Stream"/> to write to.</param>
    /// <param name="value">The value to write to the <see cref="Stream"/>.</param>
    public static unsafe void Write(this Stream stream, ReadOnlySpan<byte> buffer)
    {
        byte[] rented = ArrayPool<byte>.Shared.Rent(buffer.Length);

        buffer.CopyTo(rented);
        stream.Write(rented, 0, buffer.Length);

        ArrayPool<byte>.Shared.Return(rented);
    }

    /// <summary>
    ///     Reads a value of type <typeparamref name="TResponse"/> from a <see cref="Stream"/>
    ///     after writing a value of type <typeparamref name="TRequest"/> to the <see cref="Stream"/>.
    /// </summary>
    /// <typeparam name="TRequest">The type of value to write to the <see cref="Stream"/>.</typeparam>
    /// <typeparam name="TResponse">The type of value to read from the <see cref="Stream"/>.</typeparam>
    /// <param name="stream">The <see cref="Stream"/> to read from and write to.</param>
    /// <param name="request">The value to write to the <see cref="Stream"/>.</param>
    /// <returns>
    ///     The value read from the <see cref="Stream"/>.
    /// </returns>
    public static TResponse Transact<TRequest, TResponse>(this Stream stream, TRequest request)
        where TRequest : unmanaged
        where TResponse : unmanaged
    {
        stream.Write(request);
        return stream.Read<TResponse>();
    }

    /// <summary>
    ///     Attempts to read a value of type <typeparamref name="TResponse"/> from a <see cref="Stream"/>
    ///     after writing a value of type <typeparamref name="TRequest"/> to the <see cref="Stream"/>.
    /// </summary>
    /// <typeparam name="TRequest">The type of value to write to the <see cref="Stream"/>.</typeparam>
    /// <typeparam name="TResponse">The type of value to read from the <see cref="Stream"/>.</typeparam>
    /// <param name="stream">The <see cref="Stream"/> to read from and write to.</param>
    /// <param name="request">The value to write to the <see cref="Stream"/>.</param>
    /// <param name="response">
    ///     Contains the value read from the <see cref="Stream"/> if the operation succeeded;
    ///     otherwise, <see langword="default"/>(<typeparamref name="TResponse"/>).
    /// </param>
    /// <returns>
    ///     <see langword="true"/> if the operation succeeded;
    ///     otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryTransact<TRequest, TResponse>(this Stream stream, TRequest request, out TResponse response)
        where TRequest : unmanaged
        where TResponse : unmanaged
    {
        stream.Write(request);
        return stream.TryRead(out response);
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
            int bytesRead = read(stream, buffer[totalRead..]);

            if (bytesRead == 0)
            {
                if (throwOnEndOfStream)
                {
                    ThrowEndOfFileException();
                }

                return totalRead;
            }

            totalRead += bytesRead;
        }

        return totalRead;

        static int read(Stream stream, Span<byte> buffer)
        {
            byte[] rented = ArrayPool<byte>.Shared.Rent(buffer.Length);

            int bytesRead = stream.Read(rented, 0, buffer.Length);

            if (bytesRead > 0)
            {
                rented.AsSpan(0, bytesRead).CopyTo(buffer);
            }

            ArrayPool<byte>.Shared.Return(rented);

            return bytesRead;
        }
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
