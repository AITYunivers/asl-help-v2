using System.Diagnostics;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Memory.Pointers.Initialization;

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
                _memory = InitializeMemory(game);
                _pointers = PointerFactory.Create(_memory);
            }

            return _memory;
        }
        protected set => _memory = value;
    }

    protected override IMemoryManager InitializePipeMemory(Process process, ILogger logger, string pipeName, int timeout)
    {
        return InitializeWinApiMemory(process, logger);
    }

    protected override IMemoryManager InitializeWinApiMemory(Process process, ILogger logger)
    {
        return new WinApiMemoryManager(process, logger);
    }
}
