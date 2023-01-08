using LiveSplit.View;

namespace AslHelp.Core.LiveSplitInterop;

internal static class Timer
{
    public static void Init()
    {
        Debug.Info("  => Initializing timer data...");

        TimerForm form = (TimerForm)Application.OpenForms["TimerForm"];

        State = form.CurrentState;

        Debug.Info("    => Success.");
    }

    public static LiveSplitState State { get; private set; }

    public static IRun Run
    {
        get => State.Run;
        set => State.Run = value;
    }

    public static ILayout Layout
    {
        get => State.Layout;
        set => State.Layout = value;
    }

    public static Form Form
    {
        get => State.Form;
        set => State.Form = value;
    }

    public static TimingMethod CurrentTimingMethod
    {
        get => State.CurrentTimingMethod;
        set => State.CurrentTimingMethod = value;
    }
}
