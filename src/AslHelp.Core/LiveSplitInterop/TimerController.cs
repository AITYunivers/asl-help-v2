using System;
using LiveSplit.Model;

namespace AslHelp.Core.LiveSplitInterop;

public sealed class TimerController
{
    private readonly TimerModel _model;

    public TimerController()
    {
        _model = new() { CurrentState = Timer.State };
    }

    public void Start(Func<bool> func = null)
    {
        if (func is null || func())
        {
            _model.Start();
        }
    }

    public void Split(Func<bool> func = null)
    {
        if (func is null || func())
        {
            _model.Split();
        }
    }

    public void Reset(Func<bool> func = null)
    {
        if (func is null || func())
        {
            _model.Reset();
        }
    }

    public void Pause(Func<bool> func = null)
    {
        if (func is null || func())
        {
            _model.Reset();
        }
    }

    public void Unpause(Func<bool> func = null)
    {
        if (func is null || func())
        {
            _model.UndoAllPauses();
        }
    }

    public void Skip(Func<bool> func = null)
    {
        if (func is null || func())
        {
            _model.SkipSplit();
        }
    }

    public void Undo(Func<bool> func = null)
    {
        if (func is null || func())
        {
            _model.UndoSplit();
        }
    }
}
