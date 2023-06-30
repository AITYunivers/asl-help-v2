using System.Diagnostics;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Unity.Memory.Ipc;

public abstract partial class MonoMemoryManagerBase : MemoryManagerBase
{
    protected MonoMemoryManagerBase(Process process)
        : base(process) { }

    protected MonoMemoryManagerBase(Process process, ILogger logger)
        : base(process, logger) { }
}
