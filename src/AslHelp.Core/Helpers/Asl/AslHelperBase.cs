using System;
using System.Diagnostics;
using System.Linq;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Helpers.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Memory.Pointers;
using AslHelp.Core.Memory.Pointers.Initialization;

namespace AslHelp.Core.Helpers.Asl;

public abstract class AslHelperBase : IAslHelper
{
    public abstract IPointer this[string name] { get; set; }

    public abstract ILogger Logger { get; }

    public abstract string GameName { get; set; }
    public abstract Process? Game { get; set; }

    public abstract IMemoryManager? Memory { get; }

    public abstract IPointerFactory? Pointers { get; }

    public abstract void MapPointerValuesToCurrent();

    public void OnExit()
    {
        if (Actions.CurrentAction is not "exit")
        {
            string msg = $"Attempted to call {nameof(OnExit)} from outside the 'exit' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        OnExitImpl();
    }

    protected abstract void OnExitImpl();

    public void OnShutdown()
    {
        if (Actions.CurrentAction is not "shutdown")
        {
            string msg = $"Attempted to call {nameof(OnShutdown)} from outside the 'shutdown' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        bool closing = Debug.StackTraceNames.Any(t => t
            is "TimerForm.TimerForm_FormClosing"
            or "TimerForm.OpenLayoutFromFile"
            or "TimerForm.LoadDefaultLayout");

        OnShutdownImpl(closing);
    }

    protected abstract void OnShutdownImpl(bool closing);
}
