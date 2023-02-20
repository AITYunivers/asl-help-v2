using AslHelp.Core.Exceptions;
using AslHelp.Core.Helping;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.LiveSplitInterop.Texts;

namespace AslHelp.Neo;

public partial class Basic
    : IAslIOHelper
{
    private MultiLogger _logger;
    public ILogger Logger => _logger;

    public TimerController Timer { get; } = new();
    public TextComponentController Texts { get; } = new();
    public Dictionary<string, object> Files { get; private set; }

    public IAslIOHelper CreateFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512)
    {
        if (Methods.CurrentMethod != "startup")
        {
            string msg = "[IO] Attempted to start a file logger outside of the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        try
        {
            FileLogger logger = new(filePath, maxLines, linesToErase);
            logger.Start();

            _logger.Add(logger);
        }
        catch (Exception ex)
        {
            Debug.Error($"""
                [IO] Error creating file logger:
                {ex}
                """);
        }

        return this;
    }

    public IAslIOHelper CreateFileWatcher(string filePath)
    {
        Files ??= new();

        Files[Path.GetFileName(filePath)] = new();

        return this;
    }

    public IAslIOHelper CreateFileWatcher(string filePath, string name)
    {
        Files ??= new();

        Files[name] = new();

        return this;
    }
}
