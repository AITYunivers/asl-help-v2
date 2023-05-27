using System.Diagnostics;
using System.Runtime.CompilerServices;
using AslHelp.Common.Exceptions;
using AslHelp.Core.Helping.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Helping.Asl;

public abstract partial class AslHelperBase
{
    IMemoryManager IAslHelper.Memory
    {
        get
        {
            string action = Actions.CurrentAction;
            if (action is "startup" or "exit" or "shutdown")
            {
                string msg = $"Attempted to access the memory manager in the '{action}' action.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            return Memory;
        }
    }

    protected abstract IMemoryManager Memory
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }

    protected abstract void InitMemory(Process process);
    protected abstract void DisposeMemory();
}
