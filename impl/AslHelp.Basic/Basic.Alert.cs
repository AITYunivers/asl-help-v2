using System.Windows.Forms;

using AslHelp.Core.Diagnostics;

using LiveSplit.Model;

using LsTimer = AslHelp.Core.LiveSplitInterop.Timer;

public partial class Basic
{
    public Basic AlertRealTime()
    {
        if (LsTimer.CurrentTimingMethod == TimingMethod.RealTime)
        {
            return this;
        }

        DialogResult mbox = MessageBox.Show(LsTimer.Form,
            $"{_gameName ?? "This game"} uses real time as its timing method.\nWould you like to switch to it?",
            $"LiveSplit | {GameName}",
            MessageBoxButtons.YesNo);

        if (mbox == DialogResult.Yes)
        {
            LsTimer.CurrentTimingMethod = TimingMethod.RealTime;
        }

        return this;
    }

    public Basic AlertGameTime()
    {
        if (LsTimer.CurrentTimingMethod == TimingMethod.GameTime)
        {
            return this;
        }

        DialogResult mbox = MessageBox.Show(LsTimer.Form,
            $"{_gameName ?? "This game"} uses in-game time.\nWould you like to compare to Game Time?",
            $"LiveSplit | {GameName}",
            MessageBoxButtons.YesNo);

        if (mbox == DialogResult.Yes)
        {
            LsTimer.CurrentTimingMethod = TimingMethod.GameTime;
        }

        return this;
    }

    public Basic AlertLoadless()
    {
        if (LsTimer.CurrentTimingMethod == TimingMethod.GameTime)
        {
            return this;
        }

        DialogResult mbox = MessageBox.Show(LsTimer.Form,
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
