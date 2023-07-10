using System.Linq;

using AslHelp.Common.Exceptions;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.Memory;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase
{
    public bool Reject(params uint[] moduleMemorySizes)
    {
        string action = Actions.CurrentAction;
        if (action is not "init")
        {
            string msg = "Attempted to reject game process in the '{action}' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = Memory!.MainModule;
        if (module is null)
        {
            string msg = "MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return Reject(module, moduleMemorySizes);
    }

    public bool Reject(string moduleName, params uint[] moduleMemorySizes)
    {
        string action = Actions.CurrentAction;
        if (action is not "init")
        {
            string msg = "Attempted to reject game process in the '{action}' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Module? module = Memory!.Modules[moduleName];
        if (module is null)
        {
            string msg = $"Module '{moduleName}' was not found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return Reject(module, moduleMemorySizes);
    }

    public bool Reject(Module module, params uint[] moduleMemorySizes)
    {
        string action = Actions.CurrentAction;
        if (action is not "init")
        {
            string msg = $"Attempted to reject game process in the '{action}' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (moduleMemorySizes.Length == 0)
        {
            Game = null;
            return true;
        }

        uint memorySize = module.MemorySize;
        if (moduleMemorySizes.Any(mms => mms == memorySize))
        {
            Game = null;
            return true;
        }

        return false;
    }
}
