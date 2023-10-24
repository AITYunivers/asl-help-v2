using System.Diagnostics;
using System.Linq;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.ClickteamFusion.Memory.Ipc;

public abstract partial class ClickteamFusionMemoryManagerBase : MemoryManager, IClickteamFusionMemoryManager
{
    public ClickteamFusionMemoryManagerBase(Process process) : this(process, null) {}

    public ClickteamFusionMemoryManagerBase(Process process, ILogger? logger)
        : base(process, logger)
    {
        ClickteamFusionModule = Modules.First();
    }

    public Module ClickteamFusionModule { get; }
}