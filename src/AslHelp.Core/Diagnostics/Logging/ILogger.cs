namespace AslHelp.Core.Diagnostics.Logging;

/// <summary>
///     The <see cref="ILogger"/> interface
///     provides methods for logging information.
/// </summary>
public interface ILogger
{
    /// <summary>
    ///     Starts the <see cref="ILogger"/>.
    /// </summary>
    void Start();

    /// <summary>
    ///     Stops the <see cref="ILogger"/>.
    /// </summary>
    void Stop();

    /// <summary>
    ///     Writes an empty line to the log.
    /// </summary>
    void Log();

    /// <summary>
    ///     Writes the string representation of the specified value to the log.
    /// </summary>
    /// <param name="output">The value to log.</param>
    void Log(object? output);
}
