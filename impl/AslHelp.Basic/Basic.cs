using System;
using System.Linq;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Diagnostics;
using AslHelp.Core.LiveSplitInterop;

public partial class Basic
{
    public Basic Exit()
    {
        if (Actions.CurrentAction != "exit")
        {
            string msg = $"Attempted to call {nameof(Exit)} outside of the 'exit' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        DisposeMemory();

        for (int i = 0; i < _fileWatchers.Count; i++)
        {
            _fileWatchers[i].Dispose();
        }

        _fileWatchers.Clear();

        return this;
    }

    public Basic Shutdown()
    {
        if (Actions.CurrentAction != "shutdown")
        {
            string msg = $"Attempted to call {nameof(Shutdown)} outside of the 'shutdown' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        DisposeMemory();

        AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;

        Logger?.Stop();
        Logger?.Clear();

        for (int i = 0; i < _fileWatchers.Count; i++)
        {
            _fileWatchers[i].Dispose();
        }

        _fileWatchers.Clear();

        bool closing = Debug.StackTraceNames.Any(t => t
            is "TimerForm.TimerForm_FormClosing"
            or "TimerForm.OpenLayoutFromFile"
            or "TimerForm.LoadDefaultLayout");

        if (!closing)
        {
            Texts.RemoveAll();
        }

        return this;
    }
}
