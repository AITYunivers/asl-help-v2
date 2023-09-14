using System.Diagnostics;

using AslHelp.Core.Memory.Ipc;
using AslHelp.Mono.Memory.Ipc;
using AslHelp.Mono.Memory.MonoInterop;

using Debug = AslHelp.Core.Diagnostics.Debug;

public partial class Mono : Basic
{
    public new IMonoMemoryManager? Memory => (IMonoMemoryManager?)base.Memory;

    public IMonoManager Manager => MonoManager.Initialize(Memory!);

    protected override IMemoryManager InitializeMemory(Process process)
    {
        Debug.Info("Initiating memory...");
        return new MonoExternalMemoryManager(process, Logger);
    }
}
