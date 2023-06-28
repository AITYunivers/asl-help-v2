using System.Linq;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;

public partial class Basic
{
    public bool Reject(params int[] moduleMemorySizes)
    {
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[Reject] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.MainModule;
        if (module is null)
        {
            string msg = "[Reject] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return Reject(module, moduleMemorySizes);
    }

    public bool Reject(string moduleName, params int[] moduleMemorySizes)
    {
        IMemoryManager? memory = Memory;
        if (memory is null)
        {
            string msg = "[Reject] Process memory was not initialized.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = memory.Modules[moduleName];
        if (module is null)
        {
            string msg = $"[Reject] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return Reject(module, moduleMemorySizes);
    }

    public bool Reject(Module module, params int[] moduleMemorySizes)
    {
        if (moduleMemorySizes is null || moduleMemorySizes.Length == 0)
        {
            Game = null;
            return true;
        }

        uint exeModuleSize = module.MemorySize;
        if (moduleMemorySizes.Any(mms => mms == exeModuleSize))
        {
            Game = null;
            return true;
        }

        return false;
    }
}
