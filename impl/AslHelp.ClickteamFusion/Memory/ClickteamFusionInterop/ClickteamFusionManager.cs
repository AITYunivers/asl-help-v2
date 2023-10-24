using AslHelp.ClickteamFusion.Memory.Ipc;

namespace AslHelp.ClickteamFusion.Memory.ClickteamFusionInterop;

internal partial class ClickteamFusionManager
{
    protected readonly IClickteamFusionMemoryManager _memory;
    public nuint LoadedCCN;

    protected ClickteamFusionManager(IClickteamFusionMemoryManager memory, ClickteamFusionInitializer initializer)
    {
        _memory = memory;

        LoadedCCN = initializer.InitializeCCN(memory);
    }

    public static ClickteamFusionManager Initialize(IClickteamFusionMemoryManager memory)
    {
        return new ClickteamFusionManager(memory, new ClickteamFusionInitializer());
    }
}
