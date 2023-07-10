﻿using System;
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
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="ArgumentException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentException(string paramName, string message, [CallerMemberName] string? caller = null)
    {
        if (caller is not null)
        {
            message = $"[{caller}] {message}";
        }

        throw new ArgumentException($"[asl-help] [Error] {message}", paramName);
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
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentNullException(string paramName, string message, [CallerMemberName] string? caller = null)
    {
        if (caller is not null)
        {
            message = $"[{caller}] {message}";
        }

        throw new ArgumentNullException(paramName, $"[asl-help] [Error] {message}");
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
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentOutOfRangeException(string paramName, string message, [CallerMemberName] string? caller = null)
    {
        if (caller is not null)
        {
            message = $"[{caller}] {message}";
        }

        throw new ArgumentOutOfRangeException($"[asl-help] [Error] {message}", paramName);
    }

    /// <summary>
    ///     Throws a <see cref="DirectoryNotFoundException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="DirectoryNotFoundException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowDirectoryNotFoundException(string message, [CallerMemberName] string? caller = null)
    {
        if (caller is not null)
        {
            message = $"[{caller}] {message}";
        }

        throw new DirectoryNotFoundException($"[asl-help] [Error] {message}");
    }

    /// <summary>
    ///     Throws a <see cref="FileNotFoundException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="FileNotFoundException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowFileNotFoundException(string message, [CallerMemberName] string? caller = null)
    {
        if (caller is not null)
        {
            message = $"[{caller}] {message}";
        }

        throw new FileNotFoundException($"[asl-help] [Error] {message}");
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
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowFileNotFoundException(string fileName, string message, [CallerMemberName] string? caller = null)
    {
        if (caller is not null)
        {
            message = $"[{caller}] {message}";
        }

        throw new FileNotFoundException($"[asl-help] [Error] {message}", fileName);
    }

    /// <summary>
    ///     Throws a <see cref="FormatException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="FormatException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowFormatException(string message, [CallerMemberName] string? caller = null)
    {
        if (caller is not null)
        {
            message = $"[{caller}] {message}";
        }

        throw new FormatException($"[asl-help] [Error] {message}");
    }

    /// <summary>
    ///     Throws an <see cref="InvalidDataException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="InvalidDataException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowInvalidDataException(string message, [CallerMemberName] string? caller = null)
    {
        if (caller is not null)
        {
            message = $"[{caller}] {message}";
        }

        throw new InvalidDataException($"[asl-help] [Error] {message}");
    }

    /// <summary>
    ///     Throws an <see cref="InvalidOperationException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="InvalidOperationException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowInvalidOperationException(string message, [CallerMemberName] string? caller = null)
    {
        if (caller is not null)
        {
            message = $"[{caller}] {message}";
        }

        throw new InvalidOperationException($"[asl-help] [Error] {message}");
    }

    /// <summary>
    ///     Throws a <see cref="KeyNotFoundException"/> with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <remarks>
    ///     This method does not return and is not inlined to improve codegen of cold paths.
    /// </remarks>
    /// <exception cref="KeyNotFoundException"/>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowKeyNotFoundException(string message, [CallerMemberName] string? caller = null)
    {
        if (caller is not null)
        {
            message = $"[{caller}] {message}";
        }

        throw new KeyNotFoundException($"[asl-help] [Error] {message}");
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
