using System.IO.Pipes;
using AslHelp.Core.IO;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.IO;
using AslHelp.Core.Memory.Pipes;
using AslHelp.Core.Memory.Pointers;

public partial class Basic
{
    private NamedPipeClientStream _pipe;
    public IMemoryManager Memory { get; private set; }

    private void InitMemory()
    {
        if (_game is null)
        {
            _pipe?.Dispose();
            _pipe = null;
            Memory = null;

            return;
        }

        string dll =
            _game.Is64Bit()
            ? ResourceManager.UnpackResource("AslHelp.Core.Native.x64.dll", "x64")
            : ResourceManager.UnpackResource("AslHelp.Core.Native.x86.dll", "x86");

        if (_game.TryInject(dll))
        {
            _pipe = new("asl-help-pipe");
            _pipe.Connect();

            Memory = new PipeMemoryManager(_game, Logger, _pipe);
        }
        else
        {
            Memory = new WinApiMemoryManager(_game, Logger);
        }
    }

    private readonly Dictionary<string, IPointer> _pointers = new();
    public IPointer this[string name]
    {
        get => _pointers[name];
        set => _pointers[name] = value;
    }
}
