using System.Diagnostics;

using AslHelp.Core.Memory.Ipc;
using AslHelp.Mono.Memory.Ipc;

public partial class Mono : Basic
{
    protected override IMemoryManager InitializeMemory(Process process)
    {
        return new MonoExternalMemoryManager(process, Logger);
    }
}
