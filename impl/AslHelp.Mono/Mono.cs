using System.Diagnostics;
using System.Linq;

using AslHelp.Core.Memory.Ipc;
using AslHelp.Mono.Memory.Ipc;
using AslHelp.Mono.Memory.MonoInterop;

using Debug = AslHelp.Core.Diagnostics.Debug;

public partial class Mono : Basic
{
    // this is a problem
    public new IMonoMemoryManager? Memory => (IMonoMemoryManager?)base.Memory;

    private IMonoManager? _manager;
    private IMonoManager? Manager => _manager ??= MonoManager.Initialize(Memory!);

    public void ListImages()
    {
        var mono = new MonoEngine(Manager!);
        foreach (var klass in mono.First(img => img.Name == "Assembly-CSharp"))
        {
            Debug.Warn($"{klass.Namespace}.{klass.Name}");
        }
    }

    protected override IMemoryManager InitializeMemory(Process process)
    {
        Debug.Info("Initializing Mono-specific memory...");
        return new MonoExternalMemoryManager(process, Logger);
    }
}
