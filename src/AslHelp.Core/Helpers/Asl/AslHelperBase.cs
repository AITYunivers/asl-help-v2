using System.Linq;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Helpers.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase : IAslHelper
{
    public void OnExit()
    {
        string action = Actions.CurrentAction;
        if (action is not "exit")
        {
            string msg = $"Attempted to dispose asl-help in the '{action}' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        DisposeMemory();

        OnExitImpl();
    }

    protected abstract void OnExitImpl();

    public void OnShutdown()
    {
        string action = Actions.CurrentAction;
        if (action is not "shutdown")
        {
            string msg = $"Attempted to shut down asl-help in the '{action}' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        DisposeMemory();

        bool closing = Debug.StackTraceNames.Any(t => t
            is "TimerForm.TimerForm_FormClosing"
            or "TimerForm.OpenLayoutFromFile"
            or "TimerForm.LoadDefaultLayout");

        OnShutdownImpl(closing);
    }

    protected abstract void OnShutdownImpl(bool closing);
}
