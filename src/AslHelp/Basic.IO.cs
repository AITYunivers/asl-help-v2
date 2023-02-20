using AslHelp.Core.Exceptions;
using AslHelp.Core.Helping;
using AslHelp.Core.IO;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.LiveSplitInterop.Texts;

namespace AslHelp.Neo;

public partial class Basic
    : IAslIOStage
{
    private MultiLogger _logger;
    public ILogger Logger => _logger;

    public TimerController Timer { get; } = new();
    public TextComponentController Texts { get; } = new();
    public Dictionary<string, FileWatcher> Files { get; private set; }

    public IAslIOStage CreateFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512)
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

    public IAslIOStage CreateFileWatcher(string filePath)
    {
        Files ??= new();
        Files[Path.GetFileName(filePath)] = new(filePath);

        return this;
    }

    public IAslIOStage CreateFileWatcher(string filePath, string name)
    {
        Files ??= new();
        Files[name] = new(filePath);

        return this;
    }
}
