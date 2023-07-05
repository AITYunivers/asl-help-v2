using System.Diagnostics;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Helpers.Asl.Contracts;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Memory.Pointers;
using AslHelp.Core.Memory.Pointers.Initialization;

namespace AslHelp.Core.Helpers.Asl;

public abstract class AslHelperBase : IAslHelper
{
    public IPointer this[string name] { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public ILogger Logger => throw new System.NotImplementedException();

    public string GameName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Process? Game { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public IMemoryManager? Memory => throw new System.NotImplementedException();

    public IPointerFactory? Pointers => throw new System.NotImplementedException();

    public void MapPointerValuesToCurrent()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnShutdown()
    {
        throw new System.NotImplementedException();
    }
}
