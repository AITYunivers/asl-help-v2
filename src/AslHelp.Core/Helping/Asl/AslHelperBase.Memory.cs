using AslHelp.Core.Helping.Asl.Contracts;
using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Helping.Asl;

public abstract partial class AslHelperBase
{
    IMemoryManager IAslHelper.Memory => Memory;
    protected abstract IMemoryManager Memory
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }

    protected abstract void InitMemory(Process process);
    protected abstract void DisposeMemory();
}
