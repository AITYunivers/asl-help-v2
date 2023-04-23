using AslHelp.Core.Helping.Asl.Contracts;
using AslHelp.Core.IO;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.LiveSplitInterop.Settings;
using AslHelp.Core.LiveSplitInterop.Texts;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.Exceptions;

namespace AslHelp.Core.Helping.Asl;

public abstract partial class AslHelperBase
{
    protected readonly MultiLogger _logger = new();
    ILogger IAslHelper.Logger => _logger;

    TextComponentController IAslHelper.Texts { get; } = new();
    TimerController IAslHelper.Timer { get; } = new();
    SettingsCreator IAslHelper.Settings { get; } = new();

    protected abstract void CreateFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512);

    IAslHelper IAslHelper.CreateFileLogger(string filePath)
    {
        if (Actions.CurrentAction != "startup")
        {
            string msg = "A file logger may only be initialized in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        CreateFileLogger(filePath);

        return this;
    }

    IAslHelper IAslHelper.CreateFileLogger(string filePath, int maxLines)
    {
        if (Actions.CurrentAction != "startup")
        {
            string msg = "A file logger may only be initialized in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        CreateFileLogger(filePath, maxLines);

        return this;
    }

    IAslHelper IAslHelper.CreateFileLogger(string filePath, int maxLines, int linesToErase)
    {
        if (Actions.CurrentAction != "startup")
        {
            string msg = "A file logger may only be initialized in the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        CreateFileLogger(filePath, maxLines, linesToErase);

        return this;
    }

    protected abstract FileWatcher CreateFileWatcher(string filePath);

    FileWatcher IAslHelper.CreateFileWatcher(string filePath)
    {
        if (Actions.CurrentAction is not "startup" or "init")
        {
            string msg = "A file watcher may only be initialized in the 'startup' or 'init' actions.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        return CreateFileWatcher(filePath);
    }
}
