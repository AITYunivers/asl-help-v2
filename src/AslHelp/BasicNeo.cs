using System.Threading.Tasks;
using AslHelp.Core.Collections;
using AslHelp.Core.Helping;
using AslHelp.Core.IO;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.LiveSplitInterop.Texts;
using AslHelp.Core.Memory.IO;
using AslHelp.Core.Memory.Models;

public partial class BasicNeo
    : IAslHelper
{
    private string _gameName;
    public string GameName
    {
        get => _gameName ?? Game?.ProcessName ?? "Auto Splitter";
        set => _gameName = value;
    }

    protected Process _game;
    public virtual Process Game
    {
        get
        {
            if (_game is null || _game.HasExited)
            {
                _game = Script.Game;
            }

            _game?.Refresh();

            return _game;
        }
        set
        {
            _game = value;
            Script.Game = value;
        }
    }

    public bool Is64Bit { get; }
    public byte PtrSize { get; }
    public Module MainModule { get; }
    public ModuleCache Modules { get; }
    public IEnumerable<MemoryPage> Pages { get; }
    public Dictionary<string, FileWatcher> Files { get; }
    public TextComponentController Texts { get; }
    public TimerController Timer { get; }
    public IMemoryManager Memory { get; }
    public LoggerBase Logger { get; }

    public void Dispose()
    {

    }

    public Task Load()
    {
        return Task.Run(() =>
        {

        });
    }
}
