using System;
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
        _logger.Clear();

        for (int i = 0; i < _fileWatchers.Count; i++)
        {
            _fileWatchers[i].Dispose();
        }

        _fileWatchers.Clear();

        bool closing = Debug.Trace.ContainsAny(
            "TimerForm.TimerForm_FormClosing",
            "TimerForm.OpenLayoutFromFile",
            "TimerForm.LoadDefaultLayout");

        if (!closing)
        {
            Texts.RemoveAll();
        }
    }
}
