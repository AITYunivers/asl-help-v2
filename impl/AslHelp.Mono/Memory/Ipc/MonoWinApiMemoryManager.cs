using System.Diagnostics;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Unity.Memory.Ipc;

public sealed class UnityWinApiMemoryManager : WinApiMemoryManager
{
    public UnityWinApiMemoryManager(Process process)
        : base(process) { }

    public UnityWinApiMemoryManager(Process process, ILogger logger)
        : base(process, logger) { }
}
