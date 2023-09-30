using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;

namespace AslHelp.Common.Exceptions;

public static partial class ThrowHelper
{
    /// <summary>
    ///     Throws an <see cref="ArgumentException"/> with a specified error message
    ///     and the name of the parameter that causes this exception.
    /// </summary>
    /// <param name="paramName">The name of the parameter that caused the exception.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="caller">The name of the calling method.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentException(string paramName, string message)
    {
        throw new ArgumentException(message, paramName);
    }

    /// <summary>
    ///     Throws an <see cref="ArgumentNullException"/> with a specified error message
    ///     and the name of the parameter that causes this exception.
    /// </summary>
    /// <param name="paramName">The name of the parameter that caused the exception.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="caller">The name of the calling method.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="ArgumentNullException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentNullException(string paramName, string message)
    {
        throw new ArgumentNullException(paramName, message);
    }

    /// <summary>
    ///     Throws an <see cref="ArgumentOutOfRangeException"/> with a specified error message
    ///     and the name of the parameter that causes this exception.
    /// </summary>
    /// <param name="paramName">The name of the parameter that caused the exception.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="caller">The name of the calling method.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentOutOfRangeException(string paramName, string message)
    {
        throw new ArgumentOutOfRangeException(message, paramName);
    }

    /// <summary>
    ///     Throws a <see cref="DirectoryNotFoundException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="caller">The name of the calling method.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="DirectoryNotFoundException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowDirectoryNotFoundException(string message)
    {
        throw new DirectoryNotFoundException(message);
    }

    /// <summary>
    ///     Throws a <see cref="FileNotFoundException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="caller">The name of the calling method.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="FileNotFoundException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowFileNotFoundException(string message)
    {
        throw new FileNotFoundException(message);
    }

    /// <summary>
    ///     Throws a <see cref="FormatException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="caller">The name of the calling method.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="FormatException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowFormatException(string message)
    {
        throw new FormatException(message);
    }

    /// <summary>
    ///     Throws an <see cref="InvalidDataException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="caller">The name of the calling method.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="InvalidDataException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowInvalidDataException(string message)
    {
        throw new InvalidDataException(message);
    }

    /// <summary>
    ///     Throws an <see cref="InvalidOperationException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="caller">The name of the calling method.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="InvalidOperationException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowInvalidOperationException(string message)
    {
        throw new InvalidOperationException(message);
    }

    /// <summary>
    ///     Throws a <see cref="KeyNotFoundException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="caller">The name of the calling method.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="KeyNotFoundException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowKeyNotFoundException(string message)
    {
        throw new KeyNotFoundException(message);
    }

    /// <summary>
    ///     Throws a <see cref="Win32Exception"/>.
    /// </summary>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="Win32Exception"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowWin32Exception()
    {
        throw new Win32Exception();
    }
}
