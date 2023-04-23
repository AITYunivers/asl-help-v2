using AslHelp.Core.Exceptions;
using AslHelp.Core.Helping;
using AslHelp.Core.LiveSplitInterop;

public partial class Basic : IAslHelper
{
    public Basic() { }

    public void Exit()
    {
        if (Actions.CurrentAction != "exit")
        {
            string msg = $"Attempted to call {nameof(Exit)} outside of the 'exit' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        DisposeMemory();

        for (int i = 0; i < _files.Count; i++)
        {
            _files[i].Dispose();
        }
    }

    public void Shutdown()
    {
        if (Actions.CurrentAction != "shutdown")
        {
            string msg = $"Attempted to call {nameof(Shutdown)} outside of the 'shutdown' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

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
