using AslHelp.Core.IO.Logging;

namespace AslHelp.Core.IO;

internal static class Debug
{
    private static readonly DebugLogger _logger = new();

    public static Trace Trace { get; } = new();

    public static void Info()
    {
        _logger.Log("[asl-help]");
    }

    public static void Info(object output)
    {
        _logger.Log($"[asl-help] {output}");
    }

    public static void Warn(object output)
    {
        _logger.Log($"[Warning] {output}");
    }

    public static void Error(object output)
    {
        _logger.Log($"[Error] {output}");
    }

    public static void Throw(Exception ex)
    {
        _logger.Log("[Abort]" + Environment.NewLine + ex);
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
}
