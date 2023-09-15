using System.Diagnostics;

using AslHelp.Core.Memory.Ipc;
using AslHelp.Mono.Memory.Ipc;
using AslHelp.Mono.Memory.MonoInterop;

using Debug = AslHelp.Core.Diagnostics.Debug;

public partial class Mono : Basic
{
    // this is a problem
    public new IMonoMemoryManager? Memory => (IMonoMemoryManager?)base.Memory;

    private IMonoManager? _manager;
    public IMonoManager? Manager => _manager ??= MonoManager.Initialize(Memory!);

    protected override IMemoryManager InitializeMemory(Process process)
    {
        Debug.Info("Initializing Mono-specific memory...");
        return new MonoExternalMemoryManager(process, Logger);
    }
}
