using System.IO.Pipes;
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
            if (_memory is null)
            {
                _ = Game;
            }

            return _memory;
        }
    }

    private void InitMemory()
    {
        if (_game is null)
        {
            _pipe?.Dispose();
            _pipe = null;
            _memory = null;

            return;
        }

        string dll =
            _game.Is64Bit()
            ? ResourceManager.UnpackResource("AslHelp.Core.Native.x64.dll", "x64")
            : ResourceManager.UnpackResource("AslHelp.Core.Native.x86.dll", "x86");

        Debug.Info($"Attempting to inject {dll}...");

        if (_game.TryInject(dll))
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
