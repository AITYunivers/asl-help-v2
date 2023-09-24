using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

using AslHelp.Common.Exceptions;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.Memory;

namespace AslHelp.Core.Helpers.Asl;

public abstract partial class AslHelperBase
{
    public bool Reject(params uint[] moduleMemorySizes)
    {
        EnsureInInitForRejecting();

        return Reject(Memory.MainModule, moduleMemorySizes);
    }

    public bool Reject(string moduleName, params uint[] moduleMemorySizes)
    {
        EnsureInInitForRejecting();

        return Reject(Memory.Modules[moduleName], moduleMemorySizes);
    }

    public bool Reject(Module module, params uint[] moduleMemorySizes)
    {
        EnsureInInitForRejecting();

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

    [MemberNotNull(nameof(Memory))]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureInInitForRejecting()
    {
        string action = Actions.CurrentAction;
        if (action is not "init")
        {
            string msg = $"Attempted to reject game process in the '{action}' action.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (Memory is null)
        {
            string msg = "Attempted to access uninitialized memory.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }
    }
}
