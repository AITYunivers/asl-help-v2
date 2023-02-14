using System.IO.Pipes;
using AslHelp.Core.Collections;
using AslHelp.Core.Exceptions;
using AslHelp.Core.IO;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.IO;
using AslHelp.Core.Memory.Models;
using AslHelp.Core.Memory.Pipes;

public partial class Basic
{
    private Process _game;
    public Process Game
    {
        get
        {
            _game?.Refresh();

            if (_game is null || _game.HasExited)
            {
                _game = Script.Game;

                UpdateProcData(_game);
            }

            return _game;
        }
        set
        {
            _game = value;
            Script.Game = value;

            UpdateProcData(value);
        }
    }

    public bool Is64Bit
    {
        get
        {
            ThrowHelper.ThrowIfNull(Memory);
            return Memory.Is64Bit;
        }
    }

    public byte PtrSize
    {
        get
        {
            ThrowHelper.ThrowIfNull(Memory);
            return Memory.PtrSize;
        }
    }

    protected NamedPipeClientStream _pipe;
    public IMemoryManager Memory { get; private set; }

    public Module MainModule
    {
        get
        {
            ThrowHelper.ThrowIfNull(Memory);
            return Memory.MainModule;
        }
    }

    public ModuleCache Modules
    {
        get
        {
            ThrowHelper.ThrowIfNull(Memory);
            return Memory.Modules;
        }
    }

    public IEnumerable<MemoryPage> Pages => Game.MemoryPages(Is64Bit, true);

    private void UpdateGameData()
    {
        if (_game is null)
        {
            _pipe?.Dispose();
            _pipe = null;

            Memory = null;

            return;
        }

        string dll = ResourceManager.UnpackResource($"AslHelp.Core.Native.{(Is64Bit ? "x64" : "x86")}.dll", "Components");
        if (Injector.TryInject(_game, dll))
        {
            _pipe = new("asl-help-pipe");
            _pipe.Connect();

            Memory = new PipeMemoryManager(_game, _fileLogger is not null ? _fileLogger : _dbgLogger, _pipe);
        }
        else
        {
            Memory = new WinApiMemoryManager(_game, _fileLogger is not null ? _fileLogger : _dbgLogger);
        }
    }
}
