using System.IO.Pipes;
using AslHelp.Core;
using AslHelp.Core.IO;
using AslHelp.Core.Memory;
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
            if (_memory is null && Game is Process game)
            {
                InitMemory(game);
            }

            return _memory;
        }
    }

    private void InitMemory(Process process)
    {
        Debug.Info($"Attempting DLL injection...");

        if (process.TryInjectAslCoreNative())
        {
            Debug.Info("  => Success.");
            Debug.Info("Connecting named pipe...");

            _pipe = new("asl-help-pipe");
            _pipe.Connect();

            Debug.Info("  => Success.");

            _memory = new PipeMemoryManager(process, Logger, _pipe);
        }
        else
        {
            Debug.Info("  => Failure! Using Win32 API for memory reading.");

            _memory = new WinApiMemoryManager(process, Logger);
        }
    }

    private void DisposeMemory()
    {
        _memory?.Dispose();
        _pipe?.Dispose();
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
