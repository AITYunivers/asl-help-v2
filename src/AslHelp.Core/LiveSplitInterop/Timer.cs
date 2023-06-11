#nullable disable

using System;
using System.Windows.Forms;

using AslHelp.Common.Exceptions;

using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.View;

namespace AslHelp.Core.LiveSplitInterop;

/// <summary>
///     The <see cref="Timer"/> class
///     exposes a few LiveSplit timer-related properties.
/// </summary>
/// <remarks>
///     There can only exist one instance of the LiveSplit timer at once. This class is therefore
///     marked as <see langword="static"/>. Call <see cref="Init"/> to initialize the relevant data.
/// </remarks>
internal static class Timer
{
    private static LiveSplitState _state;

    /// <summary>
    ///     Retrieves the current LiveSplit timer's <see cref="TimerForm"/> and its associated <see cref="LiveSplitState"/>.
    /// </summary>
    /// <remarks>
    ///     This method can only succeed when the current application is in fact LiveSplit.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when no <see cref="System.Windows.Forms.Form"/> with the name "TimerForm" can be found.
    /// </exception>
    public static void Init()
    {
        Debug.Info("  => Initializing timer data...");

        TimerForm form = (TimerForm)Application.OpenForms[nameof(TimerForm)];
        if (form is null)
        {
            string msg = "Could not find the LiveSplit timer form.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        _state = form.CurrentState;

        Debug.Info("    => Success.");
    }

    /// <summary>
    ///     Gets the timer's <see cref="LiveSplitState"/>.
    /// </summary>
    public static LiveSplitState State { get; private set; }

    /// <summary>
    ///     Gets or sets the timer's <see cref="IRun"/>.
    /// </summary>
    public static IRun Run { get; set; }

    /// <summary>
    ///     Gets or sets the timer's <see cref="ILayout"/>.
    /// </summary>
    public static ILayout Layout { get; set; }

    /// <summary>
    ///     Gets or sets the timer's <see cref="TimerForm"/>.
    /// </summary>
    public static Form Form { get; set; }

    /// <summary>
    ///     Gets or sets the timer's <see cref="TimingMethod"/>.
    /// </summary>
    public static TimingMethod CurrentTimingMethod { get; set; }
}
