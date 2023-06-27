using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using AslHelp.Core.Diagnostics.Logging;

namespace AslHelp.Core.Diagnostics;

internal static class Debug
{
    private static readonly DebugLogger _logger = new();

    public static void Info()
    {
        _logger.Log("[asl-help]");
    }

    public static void Info(object output)
    {
        _logger.Log($"[asl-help] [Info] {output}");
    }

    public static void Warn(object output)
    {
        _logger.Log($"[asl-help] [Warn] {output}");
    }

    public static void Error(object output)
    {
        _logger.Log($"[asl-help] [Error] {output}");
    }

    public static void Throw(Exception ex)
    {
        _logger.Log($"""
            [asl-help] [Abort]
            {ex}
            """);
    }

    public static DialogResult Show(string message)
    {
        return MessageBox.Show(Timer.Form, message, "LiveSplit | asl-help");
    }

    public static DialogResult Show(string message, MessageBoxButtons buttons)
    {
        return MessageBox.Show(Timer.Form, message, "LiveSplit | asl-help", buttons);
    }

    public static DialogResult Show(string message, MessageBoxButtons buttons, MessageBoxIcon icon)
    {
        return MessageBox.Show(Timer.Form, message, "LiveSplit | asl-help", buttons, icon);
    }

    public static IEnumerable<string> StackTraceNames
    {
        get => new StackTrace()
            .GetFrames()
            .Select(f =>
            {
                MethodBase method = f.GetMethod();
                Type decl = method.DeclaringType;

                return decl is null ? method.Name : $"{decl.Name}.{method.Name}";
            });
    }
}
