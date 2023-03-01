using AslHelp.Core.Exceptions;
using AslHelp.Core.Helping;
using AslHelp.Core.IO;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.LiveSplitInterop.Settings;
using AslHelp.Core.LiveSplitInterop.Texts;

public partial class Basic
{
    private readonly List<FileWatcher> _files = new();

    private MultiLogger _logger;
    public ILogger Logger => _logger;

    public TimerController Timer { get; } = new();
    public TextComponentController Texts { get; } = new();
    public SettingsCreator Settings { get; } = new();

    public void CreateFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512)
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
    }

    public FileWatcher CreateFileWatcher(string filePath)
    {
        FileWatcher watcher = new(filePath);
        _files.Add(watcher);

        return watcher;
    }
}
