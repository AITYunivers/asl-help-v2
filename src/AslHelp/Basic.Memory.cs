using AslHelp.Core.Memory.Injection;
using AslHelp.Core.Memory.IO;

public partial class Basic
{
    private IMemoryManager _memory;
    public IMemoryManager Memory
    {
        get
        {
            EnsureInitialized();

            if (_memory is null && Game is Process game)
            {
                InitMemory(game);
            }

            return _memory;
        }
    }

    private void InitMemory(Process process)
    {
        Debug.Info("Initiating memory...");

        if (_withInjection)
        {
            Debug.Info("  => Attempting to inject...");

            if (process.TryInjectAslCoreNative())
            {
                Debug.Info("  => Success.");
                Debug.Info("  => Connecting named pipe...");

                try
                {
                    _memory = new PipeMemoryManager(process, Logger, "asl-help-pipe", _timeout);

                    Debug.Info("    => Success.");

                    return;
                }
                catch (TimeoutException)
                {
                    Debug.Warn("  => Timed out!");
                }
            }
            else
            {
                Debug.Warn("  => Failure!");
            }
        }

        Debug.Info("  => Using Win32 API for memory reading.");

        _memory = new WinApiMemoryManager(process, Logger);
    }

    private void DisposeMemory()
    {
        _memory?.Dispose();
        _memory = null;

        _game?.Dispose();
        _game = null;

        _pointers = null;
    }
}
