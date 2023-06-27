using System.Collections.Generic;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.IO;
using AslHelp.Core.LiveSplitInterop.Control;
using AslHelp.Core.LiveSplitInterop.Settings;

public partial class Basic
{
    private readonly List<FileWatcher> _fileWatchers = new();

    private readonly MultiLogger _logger = new();
    protected override ILogger Logger => _logger;

    protected override TimerController Timer { get; } = new();
    protected override SettingsCreator Settings { get; } = new();
    protected override TextComponentController Texts { get; } = new();

    protected override void CreateFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512)
    {
        FileLogger logger = new(filePath, maxLines, linesToErase);
        logger.Start();

        _logger.Add(logger);
    }

    protected override FileWatcher CreateFileWatcher(string filePath)
    {
        FileWatcher watcher = new(filePath);
        _fileWatchers.Add(watcher);

        return watcher;
    }
}
