using System.Diagnostics;
using System.Linq;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Unity.Memory.Ipc;

public abstract partial class MonoMemoryManagerBase : MemoryManagerBase, IMonoMemoryManager
{
    public MonoMemoryManagerBase(Process process)
        : base(process)
    {
        MonoModule = Modules.Single(m => m.Name is "mono.dll" or "mono-2.0-bdwgc.dll" or "GameAssembly.dll");
    }

    public MonoMemoryManagerBase(Process process, ILogger logger)
        : base(process, logger)
    {
        MonoModule = Modules.Single(m => m.Name is "mono.dll" or "mono-2.0-bdwgc.dll" or "GameAssembly.dll");
    }

    public Module MonoModule { get; }
}
