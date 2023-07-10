using System.Windows.Forms;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Helpers.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;

using LiveSplit.Model;

using LsTimer = AslHelp.Core.LiveSplitInterop.Timer;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase
{
    public IAslHelper.Initialization AlertRealTime(string? message = null)
    {
        string action = Actions.CurrentAction;
        if (action is not "startup")
        {
            string msg = $"Attempted to alert user in the '{action}' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (LsTimer.CurrentTimingMethod == TimingMethod.RealTime)
        {
            return this;
        }

        DialogResult mbox = MessageBox.Show(LsTimer.Form,
            message ??
            $"{_gameName ?? "This game"} uses real time as its timing method.\nWould you like to switch to it?",
            $"LiveSplit | {GameName}",
            MessageBoxButtons.YesNo);

        if (mbox == DialogResult.Yes)
        {
            LsTimer.CurrentTimingMethod = TimingMethod.RealTime;
        }

        return this;
    }

    public IAslHelper.Initialization AlertGameTime(string? message = null)
    {
        string action = Actions.CurrentAction;
        if (action is not "startup")
        {
            string msg = $"Attempted to alert user in the '{action}' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (LsTimer.CurrentTimingMethod == TimingMethod.GameTime)
        {
            return this;
        }

        DialogResult mbox = MessageBox.Show(LsTimer.Form,
            message ??
            $"{_gameName ?? "This game"} uses in-game time.\nWould you like to compare to Game Time?",
            $"LiveSplit | {GameName}",
            MessageBoxButtons.YesNo);

        if (mbox == DialogResult.Yes)
        {
            LsTimer.CurrentTimingMethod = TimingMethod.GameTime;
        }

        return this;
    }

    public IAslHelper.Initialization AlertLoadless(string? message = null)
    {
        string action = Actions.CurrentAction;
        if (action is not "startup")
        {
            string msg = $"Attempted to alert user in the '{action}' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (LsTimer.CurrentTimingMethod == TimingMethod.GameTime)
        {
            return this;
        }

        DialogResult mbox = MessageBox.Show(LsTimer.Form,
            message ??
            $"Removing loads from {_gameName ?? "this game"} requires comparing against Game Time.\nWould you like to switch to it?",
            $"LiveSplit | {GameName}",
            MessageBoxButtons.YesNo);

        if (mbox == DialogResult.Yes)
        {
            LsTimer.CurrentTimingMethod = TimingMethod.GameTime;
        }

        return this;
    }
}
