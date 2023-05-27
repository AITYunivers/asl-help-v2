using System.Runtime.CompilerServices;
using AslHelp.Common.Exceptions;
using AslHelp.Core.Helping.Asl.Contracts;
using AslHelp.Core.IO;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.LiveSplitInterop.Settings;
using AslHelp.Core.LiveSplitInterop.Texts;

namespace AslHelp.Core.Helping.Asl;

public abstract partial class AslHelperBase
{
    ILogger IAslHelper.Logger => Logger;
    protected abstract ILogger Logger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }

    TextComponentController IAslHelper.Texts => Texts;
    protected abstract TextComponentController Texts
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }

    TimerController IAslHelper.Timer => Timer;
    protected abstract TimerController Timer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }

    SettingsCreator IAslHelper.Settings
    {
        get
        {
            if (Actions.CurrentAction != "startup")
            {
                string msg = $"Attempted to access the settings creator outside of the 'startup' action.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            return Settings;
        }
    }

    protected abstract SettingsCreator Settings
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }

    protected abstract void CreateFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512);

    IAslHelper IAslHelper.CreateFileLogger(string filePath)
    {
        if (Actions.CurrentAction != "startup")
        {
            string msg = "A file logger may only be initialized in the 'startup' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        CreateFileLogger(filePath);

        return this;
    }

    IAslHelper IAslHelper.CreateFileLogger(string filePath, int maxLines)
    {
        if (Actions.CurrentAction != "startup")
        {
            string msg = "A file logger may only be initialized in the 'startup' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        CreateFileLogger(filePath, maxLines);

        return this;
    }

    IAslHelper IAslHelper.CreateFileLogger(string filePath, int maxLines, int linesToErase)
    {
        if (Actions.CurrentAction != "startup")
        {
            string msg = "A file logger may only be initialized in the 'startup' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
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
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return CreateFileWatcher(filePath);
    }
}
