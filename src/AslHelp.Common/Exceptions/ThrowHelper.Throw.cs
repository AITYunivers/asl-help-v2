using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentException(string paramName, string message)
    {
        throw new ArgumentException(message, paramName);
    }

    /// <summary>
    ///     Throws an <see cref="ArgumentNullException"/> with the name of the parameter that causes this exception.
    /// </summary>
    /// <param name="paramName">The name of the parameter that caused the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="ArgumentNullException"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentNullException(string paramName)
    {
        throw new ArgumentNullException(paramName);
    }

    /// <summary>
    ///     Throws an <see cref="ArgumentNullException"/> with a specified error message
    ///     and the name of the parameter that causes this exception.
    /// </summary>
    /// <param name="paramName">The name of the parameter that caused the exception.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="ArgumentNullException"/>
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
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentOutOfRangeException(string paramName, string message)
    {
        throw new ArgumentOutOfRangeException(message, paramName);
    }

    /// <summary>
    ///     Throws a <see cref="DirectoryNotFoundException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="DirectoryNotFoundException"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowDirectoryNotFoundException(string message)
    {
        throw new DirectoryNotFoundException(message);
    }

    /// <summary>
    ///     Throws a <see cref="FileNotFoundException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="FileNotFoundException"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowFileNotFoundException(string message)
    {
        throw new FileNotFoundException(message);
    }

    /// <summary>
    ///     Throws a <see cref="FileNotFoundException"/> with a specified error message
    ///     and the name of the file name that cannot be found.
    /// </summary>
    /// <param name="fileName">The full name of the file that cannot be found.</param>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="FileNotFoundException"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowFileNotFoundException(string fileName, string message)
    {
        throw new FileNotFoundException(message, fileName);
    }

    /// <summary>
    ///     Throws a <see cref="FormatException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="FormatException"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowFormatException(string message)
    {
        throw new FormatException(message);
    }

    /// <summary>
    ///     Throws an <see cref="InvalidDataException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="InvalidDataException"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowInvalidDataException(string message)
    {
        throw new InvalidDataException(message);
    }

    /// <summary>
    ///     Throws an <see cref="InvalidOperationException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="InvalidOperationException"/>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowInvalidOperationException(string message)
    {
        throw new InvalidOperationException(message);
    }

    /// <summary>
    ///     Throws a <see cref="KeyNotFoundException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="KeyNotFoundException"/>
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
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowWin32Exception()
    {
        throw new Win32Exception();
    }
}
