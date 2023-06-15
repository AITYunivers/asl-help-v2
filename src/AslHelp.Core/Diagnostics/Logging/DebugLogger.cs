using System.Diagnostics;

namespace AslHelp.Core.Diagnostics.Logging;

public sealed class DebugLogger : ILogger
{
    /// <summary>
    ///     Not implemented.
    /// </summary>
    public void Start() { }

    /// <summary>
    ///     Writes an empty line to LiveSplit's <see cref="EventLogTraceListener"/>.
    /// </summary>
    public void Log()
    {
        LiveSplit.Options.Log.Info("");
    }

    /// <summary>
    ///     Writes the specified value to LiveSplit's <see cref="EventLogTraceListener"/>.
    /// </summary>
    /// <param name="output">
    ///     The value to log.
    /// </param>
    public void Log(object? output)
    {
        LiveSplit.Options.Log.Info(output?.ToString());
    }

    /// <summary>
    ///     Not implemented.
    /// </summary>
    public void Stop() { }
}
