using System.Collections.Generic;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.IO;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.LiveSplitInterop.Control;
using AslHelp.Core.LiveSplitInterop.Settings;

public partial class Basic
{
    private readonly List<FileWatcher> _fileWatchers = new();

    public MultiLogger Logger = new();
    public TimerController Timer { get; } = new();
    public TextComponentController Texts { get; } = new();

    private readonly SettingsCreator _settings = new();
    public SettingsCreator Settings
    {
        get
        {
            if (Actions.CurrentAction != "startup")
            {
                string msg = $"Attempted to access the settings creator outside of the 'startup' action.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            return _settings;
        }
    }

    public Basic CreateFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512)
    {
        if (Actions.CurrentAction != "startup")
        {
            string msg = "A file logger may only be initialized in the 'startup' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        FileLogger logger = new(filePath, maxLines, linesToErase);
        logger.Start();

        Logger.Add(logger);

        return this;
    }

    public FileWatcher CreateFileWatcher(string filePath)
    {
        if (Actions.CurrentAction is not "startup" or "init")
        {
            string msg = "A file watcher may only be initialized in the 'startup' or 'init' actions.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        FileWatcher watcher = new(filePath);
        _fileWatchers.Add(watcher);

        return watcher;
    }
}
