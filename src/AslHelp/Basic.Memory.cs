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
        (string resource, string dll) = _game.Is64Bit()
            ? (AHR.NativeLib64, Path.GetFullPath($"./x64/{AHR.NativeLib64}"))
            : (AHR.NativeLib86, Path.GetFullPath($"./x86/{AHR.NativeLib86}"));

        if (!_game.DllIsInjected(dll))
        {
            ResourceManager.UnpackResource(resource, dll);
        }

        Debug.Info($"Attempting to inject {resource}...");

        if (_game.TryInjectDll(dll))
        {
            Debug.Info("  => Success.");
            Debug.Info("Connecting named pipe...");

            _pipe = new("asl-help-pipe");
            _pipe.Connect();

            Debug.Info("  => Success.");

            _memory = new PipeMemoryManager(_game, Logger, _pipe);
        }
        else
        {
            Debug.Info("  => Failure! Using Win32 API for memory reading.");

            _memory = new WinApiMemoryManager(_game, Logger);
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
