using System;

using LiveSplit.Model;

namespace AslHelp.Core.LiveSplitInterop;

/// <summary>
///     The <see cref="TimerController"/> class
///     contains methods for controlling the LiveSplit timer.
/// </summary>
public sealed class TimerController
{
    private readonly TimerModel _model;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TimerController"/> class.
    /// </summary>
    public TimerController()
    {
        _model = new() { CurrentState = Timer.State };
    }

    /// <summary>
    ///     Starts the LiveSplit timer. A function can optionally be passed to
    ///     evaluate whether the timer should be started.
    /// </summary>
    /// <param name="func">
    ///     The function used to evaluate whether the timer should be started.
    /// </param>
    public void Start(Func<bool>? func = null)
    {
        if (func is null || func())
        {
            _model.Start();
        }
    }

    /// <summary>
    ///     Splits the LiveSplit timer. A function can optionally be passed to
    ///     evaluate whether the timer should split.
    /// </summary>
    /// <param name="func">
    ///     The function used to evaluate whether the timer should split.
    /// </param>
    public void Split(Func<bool>? func = null)
    {
        if (func is null || func())
        {
            _model.Split();
        }
    }

    /// <summary>
    ///     Resets the LiveSplit timer. A function can optionally be passed to
    ///     evaluate whether the timer should be reset.
    /// </summary>
    /// <param name="func">
    ///     The function used to evaluate whether the timer should be reset.
    /// </param>
    public void Reset(Func<bool>? func = null)
    {
        if (func is null || func())
        {
            _model.Reset();
        }
    }

    /// <summary>
    ///     Pauses the LiveSplit timer. A function can optionally be passed to
    ///     evaluate whether the timer should be paused.
    /// </summary>
    /// <param name="func">
    ///     The function used to evaluate whether the timer should be paused.
    /// </param>
    public void Pause(Func<bool>? func = null)
    {
        if (func is null || func())
        {
            _model.Reset();
        }
    }

    /// <summary>
    ///     Unpauses the LiveSplit timer. A function can optionally be passed to
    ///     evaluate whether the timer should be unpaused.
    /// </summary>
    /// <param name="func">
    ///     The function used to evaluate whether the timer should be unpaused.
    /// </param>
    public void Unpause(Func<bool>? func = null)
    {
        if (func is null || func())
        {
            _model.UndoAllPauses();
        }
    }

    /// <summary>
    ///     Skips a split. A function can optionally be passed to
    ///     evaluate whether the split should be skipped.
    /// </summary>
    /// <param name="func">
    ///     The function used to evaluate whether the split should be skipped.
    /// </param>
    public void Skip(Func<bool>? func = null)
    {
        if (func is null || func())
        {
            _model.SkipSplit();
        }
    }

    /// <summary>
    ///     Undoes a split. A function can optionally be passed to
    ///     evaluate whether the split should be undone.
    /// </summary>
    /// <param name="func">
    ///     The function used to evaluate whether the split should be undone.
    /// </param>
    public void Undo(Func<bool>? func = null)
    {
        if (func is null || func())
        {
            _model.UndoSplit();
        }
    }
}
