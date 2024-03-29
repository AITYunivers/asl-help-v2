using System.Diagnostics;

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
                Debug.Info("Initializing memory...");

                _memory = InitializeMemory(game);

                Debug.Info("  => Done.");
            }

            return _memory;
        }
        protected set => _memory = value;
    }

    protected virtual IMemoryManager InitializeMemory(Process process)
    {
        return new ExternalMemoryManager(process, Logger);
    }

    protected override void DisposeProcessInstanceData()
    {
        base.DisposeProcessInstanceData();

        _game = null;
        _pointers = null;
    }
}
