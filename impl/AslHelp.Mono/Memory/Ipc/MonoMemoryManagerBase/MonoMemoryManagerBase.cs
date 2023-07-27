using System.Diagnostics;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Mono.Memory.Ipc;

public abstract partial class MonoMemoryManagerBase : MemoryManagerBase, IMonoMemoryManager
{
    public MonoMemoryManagerBase(Process process)
        : base(process) { }

    public MonoMemoryManagerBase(Process process, ILogger logger)
        : base(process, logger) { }
}
