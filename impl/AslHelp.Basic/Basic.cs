using System;
using System.Linq;

using AslHelp.Core.Diagnostics;
using AslHelp.Core.Helping.Asl;

public partial class Basic : AslHelperBase
{
    protected override void Exit()
    {
        for (int i = 0; i < _fileWatchers.Count; i++)
        {
            _fileWatchers[i].Dispose();
        }

        _fileWatchers.Clear();
    }

    protected override void Shutdown()
    {
        AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;

        _logger?.Stop();
        _logger?.Clear();

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
    }
}
