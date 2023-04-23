using AslHelp.Core.Helping.Asl;
using AslHelp.Core.Helping.Asl.Contracts;

public partial class Basic
    : AslHelperBase
{
    protected override void Exit()
    {
        DisposeMemory();

        for (int i = 0; i < _fileWatchers.Count; i++)
        {
            _fileWatchers[i].Dispose();
        }

        _fileWatchers.Clear();
    }

    protected override void Shutdown()
    {
        DisposeMemory();

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
