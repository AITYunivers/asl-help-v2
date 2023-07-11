using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using AslHelp.Core.Diagnostics.Logging;

namespace AslHelp.Core.Diagnostics;

/// <summary>
///     The <see cref="Debug"/> class
///     provides diagnostic methods for the application.
/// </summary>
internal static class Debug
{
    private static readonly DebugLogger _logger = new();

    /// <summary>
    ///     Writes an empty trace log.
    /// </summary>
    public static void Info()
    {
        _logger.Log("[asl-help]");
    }

    /// <summary>
    ///     Writes the string representation of the specified value to the trace log as an information.
    /// </summary>
    /// <param name="output">The value to log.</param>
    public static void Info(object? output)
    {
        _logger.Log($"[asl-help] [Info] {output}");
    }

    /// <summary>
    ///     Writes the string representation of the specified value to the trace log as a warning.
    /// </summary>
    /// <param name="output">The value to log.</param>
    public static void Warn(object? output)
    {
        _logger.Log($"[asl-help] [Warn] {output}");
    }

    /// <summary>
    ///     Writes the string representation of the specified value to the trace log as an error.
    /// </summary>
    /// <param name="output">The value to log.</param>
    public static void Error(object? output)
    {
        _logger.Log($"[asl-help] [Error] {output}");
    }

    /// <summary>
    ///     Writes an <see cref="Exception"/> to the trace log as a fatal error.
    /// </summary>
    /// <param name="ex">The <see cref="Exception"/> to log.</param>
    public static void Throw(Exception ex)
    {
        _logger.Log($"""
            [asl-help] [Abort]
            {ex}
            """);
    }

    /// <summary>
    ///     Shows a message box with the specified message.
    /// </summary>
    /// <param name="message">The message to show.</param>
    /// <returns>
    ///     The <see cref="DialogResult"/> value that corresponds to the button the user clicked.
    /// </returns>
    public static DialogResult Show(string message)
    {
        return MessageBox.Show(Timer.Form, message, "LiveSplit | asl-help");
    }

    /// <summary>
    ///     Shows a message box with the specified message and buttons.
    /// </summary>
    /// <param name="message">The message to show.</param>
    /// <param name="buttons">The buttons to show.</param>
    /// <returns>
    ///     The <see cref="DialogResult"/> value that corresponds to the button the user clicked.
    /// </returns>
    public static DialogResult Show(string message, MessageBoxButtons buttons)
    {
        return MessageBox.Show(Timer.Form, message, "LiveSplit | asl-help", buttons);
    }

    /// <summary>
    ///     Shows a message box with the specified message, buttons, and icon.
    /// </summary>
    /// <param name="message">The message to show.</param>
    /// <param name="buttons">The buttons to show.</param>
    /// <param name="icon">The icon to show.</param>
    /// <returns>
    ///     The <see cref="DialogResult"/> value that corresponds to the button the user clicked.
    /// </returns>
    public static DialogResult Show(string message, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        return MessageBox.Show(Timer.Form, message, "LiveSplit | asl-help", buttons, icon);
    }

    /// <summary>
    ///     Gets the fully qualified names of the methods in the call stack.
    /// </summary>
    public static IEnumerable<string> StackTraceNames
    {
        get => new StackTrace(6)
            .GetFrames()
            .Select(f =>
            {
                MethodBase method = f.GetMethod();
                Type decl = method.DeclaringType;

                return decl is null ? method.Name : $"{decl.Name}.{method.Name}";
            });
    }
}
