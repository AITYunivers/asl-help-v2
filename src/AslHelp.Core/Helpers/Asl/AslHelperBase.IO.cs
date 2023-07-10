using AslHelp.Common.Exceptions;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Helpers.Asl.Contracts;
using AslHelp.Core.IO;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.LiveSplitInterop.Control;
using AslHelp.Core.LiveSplitInterop.Settings;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase
{
    public abstract ILogger Logger { get; }
    public abstract SettingsCreator Settings { get; }

    protected abstract IAslHelper.Initialization LogToFileImpl(string fileName, int maxLines, int linesToErase);
    protected abstract FileWatcher CreateFileWatcherImpl(string fileName);

    public TimerController Timer { get; } = new();
    public TextComponentController Texts { get; } = new();

    public IAslHelper.Initialization LogToFile(string fileName, int maxLines = 4096, int linesToErase = 512)
    {
        if (_initialized)
        {
            string msg = "Logging to a file may only be enabled before initialization.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (Actions.CurrentAction is not "startup")
        {
            string msg = "Logging to a file may only be enabled in the 'startup' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return LogToFileImpl(fileName, maxLines, linesToErase);
    }

    public FileWatcher CreateFileWatcher(string fileName)
    {
        if (Actions.CurrentAction is not "startup" or "init")
        {
            string msg = "File watchers may only be created in the 'startup' or 'init' actions.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return CreateFileWatcherImpl(fileName);
    }
}
