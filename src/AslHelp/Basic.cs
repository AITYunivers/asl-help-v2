using AslHelp.Core.Helping;
using AslHelp.Core.Reflection;

public partial class Basic : IAslHelper
{
    public Basic() { }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;

        DisposeMemory();

        _logger?.Stop();
        _logger = null;

        for (int i = 0; i < _files.Count; i++)
        {
            _files[i].Dispose();
        }

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
