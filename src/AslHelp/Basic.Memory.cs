using AslHelp.Core.Collections;
using AslHelp.Core.Exceptions;
using AslHelp.Core.IO;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.IO;
using AslHelp.Core.Memory.Models;
using AslHelp.Core.Memory.Pipes;
using System.IO.Pipes;

namespace AslHelp.Neo;

public partial class Basic
{
    private NamedPipeClientStream _pipe;
    public IMemoryManager Memory { get; private set; }

    public bool Is64Bit => Memory.Is64Bit;
    public byte PtrSize => Memory.PtrSize;

    public Module MainModule => Memory.MainModule;
    public ModuleCache Modules => Memory.Modules;

    public IEnumerable<MemoryPage> Pages(bool allPages)
    {
        ThrowHelper.ThrowIfNull(Game);

        return Native.MemoryPages(Game.Handle, Is64Bit, allPages);
    }

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
}
