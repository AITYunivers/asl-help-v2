using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.ClickteamFusion.Memory.Ipc;

public interface IClickteamFusionMemoryManager : IMemoryManager
{
    Module ClickteamFusionModule { get; }
}
