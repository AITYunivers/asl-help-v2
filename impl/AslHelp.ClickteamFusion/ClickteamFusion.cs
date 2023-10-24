using System.Diagnostics;
using System.Linq;

using AslHelp.Core.Memory.Ipc;
using AslHelp.ClickteamFusion.Memory.Ipc;
using AslHelp.ClickteamFusion.Memory.ClickteamFusionInterop;

using System.Threading;

public partial class ClickteamFusion : Basic
{
    public new IClickteamFusionMemoryManager? Memory => (IClickteamFusionMemoryManager?)base.Memory;
    private ClickteamFusionManager? _manager;

    public CRunApp? App;
    public CRunFrame? Frame;

    public void LoadCCN()
    {
        while (Memory?.Pages(false).Count() < 30)
        {
            Thread.Sleep(100);
        }

        _manager = ClickteamFusionManager.Initialize(Memory!);
        App = new CRunApp(_manager.LoadedCCN, Memory!);
        Frame = new CRunFrame(_manager.LoadedCCN, Memory!);
    }

    public bool IsLoading => App != null && App.LoadedFrame == -1;

    protected override IMemoryManager InitializeMemory(Process process)
    {
        Debug.Info("Initializing Clickteam Fusion-specific memory...");
        return new ClickteamFusionExternalMemoryManager(process, Logger);
    }
}
