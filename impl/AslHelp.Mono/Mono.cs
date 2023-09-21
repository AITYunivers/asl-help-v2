using System.Diagnostics;

using AslHelp.Core.Memory.Ipc;
using AslHelp.Mono.Memory.Ipc;

using Debug = AslHelp.Core.Diagnostics.Debug;

public partial class Mono : Basic
{
    protected override IMemoryManager InitializeMemory(Process process)
    {
        Debug.Info("Initializing Mono-specific memory...");
        return new MonoExternalMemoryManager(process, Logger);
    }
}
