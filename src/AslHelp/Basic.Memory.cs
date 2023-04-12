using System.IO.Pipes;
using AslHelp.Core.Memory.IO;
using AslHelp.Core.Memory.Pipes;
using AslHelp.Core.Memory.Pointers;

public partial class Basic
{
    private NamedPipeClientStream _pipe;
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

                _pipe = new("asl-help-pipe");
                _pipe.Connect();

                Debug.Info("    => Success.");

                _memory = new PipeMemoryManager(process, Logger, _pipe);

                return;
            }

            Debug.Info("  => Failure!");
        }

        Debug.Warn("  => Using Win32 API for memory reading.");

        _memory = new WinApiMemoryManager(process, Logger);
    }

    private void DisposeMemory()
    {
        _memory?.Dispose();
        _memory = null;

        _pipe?.Dispose();
        _pipe = null;

        _game?.Dispose();
        _game = null;
    }

    private readonly Dictionary<string, IPointer> _pointers = new();
    public IPointer this[string name]
    {
        get => _pointers[name];
        set => _pointers[name] = value;
    }

    public void MapPointerValuesToCurrent()
    {
        foreach (KeyValuePair<string, IPointer> entry in _pointers)
        {
            Script.Current[entry.Key] = entry.Value.Current;
        }
    }
}
