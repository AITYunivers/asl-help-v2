using System.Diagnostics;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Memory.Pointers.Initialization;

using Debug = AslHelp.Core.Diagnostics.Debug;

public partial class Basic
{
    private IMemoryManager? _memory;
    public override IMemoryManager? Memory
    {
        get
        {
            if (_memory is not null)
            {
                return _memory;
            }

            if (Game is Process game)
            {
                _memory = InitializeMemory(game, Logger);
                _pointers = PointerFactory.Create(_memory);
            }

            return _memory;
        }
        protected set => _memory = value;
    }

    protected override IMemoryManager InitializeMemory(Process process, ILogger logger)
    {
        Debug.Info("  => Initializing memory...");

        return new WinApiMemoryManager(process, logger);
    }
}
