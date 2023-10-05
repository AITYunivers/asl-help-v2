using System.Linq;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Helpers.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase : IAslHelper
{
    protected const string LibraryName = "asl-help-v2";

    public void OnExit()
    {
        string action = Actions.CurrentAction;
        if (action is not "exit")
        {
            string msg = $"Attempted to dispose asl-help in the '{action}' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Debug.Info("Disposing...");

        DisposeProcessInstanceData();

        OnExitImpl();
    }

    /// <summary>
    ///     This method is called from <see cref="OnExit"/> after some checks are performed.
    /// </summary>
    protected abstract void OnExitImpl();

    public void OnShutdown()
    {
        string action = Actions.CurrentAction;
        if (action is not "shutdown")
        {
            string msg = $"Attempted to shut down asl-help in the '{action}' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Debug.Info("Disposing...");

        DisposeProcessInstanceData();

        bool closing = Debug.StackTraceNames.Any(t => t
            is "TimerForm.TimerForm_FormClosing"
            or "TimerForm.OpenLayoutFromFile"
            or "TimerForm.LoadDefaultLayout");

        OnShutdownImpl(closing);
    }

    /// <summary>
    ///     This method is called from <see cref="OnShutdown"/> after some checks are performed.
    /// </summary>
    /// <param name="closing">Specifies whether LiveSplit is closing.</param>
    protected abstract void OnShutdownImpl(bool closing);
}
