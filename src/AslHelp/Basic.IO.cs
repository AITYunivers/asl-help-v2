using AslHelp.Core.Exceptions;
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

    private readonly TimerController _timer = new();
    public TimerController Timer
    {
        get
        {
            EnsureInitialized();
            return _timer;
        }
    }

    private readonly TextComponentController _texts = new();
    public TextComponentController Texts
    {
        get
        {
            EnsureInitialized();
            return _texts;
        }
    }

    private readonly SettingsCreator _settings = new();
    public SettingsCreator Settings
    {
        get
        {
            EnsureInitialized();
            return _settings;
        }
    }

    public void CreateFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512)
    {
        EnsureInitialized();

        if (Methods.CurrentMethod != "startup")
        {
            string msg = "A file logger may only be initialized in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        FileLogger logger = new(filePath, maxLines, linesToErase);
        logger.Start();

        _logger.Add(logger);
    }

    public FileWatcher CreateFileWatcher(string filePath)
    {
        EnsureInitialized();

        if (Methods.CurrentMethod is not "startup" or "init")
        {
            string msg = "A file watcher may only be initialized in the 'startup' or 'init' actions.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        FileWatcher watcher = new(filePath);
        _files.Add(watcher);

        return watcher;
    }
}
