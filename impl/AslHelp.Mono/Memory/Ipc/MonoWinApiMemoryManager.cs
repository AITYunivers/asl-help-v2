using System.Diagnostics;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Unity.Memory.Ipc;

public sealed class MonoWinApiMemoryManager : WinApiMemoryManager
{
    public MonoWinApiMemoryManager(Process process)
        : base(process) { }

    public MonoWinApiMemoryManager(Process process, ILogger logger)
        : base(process, logger) { }
}
