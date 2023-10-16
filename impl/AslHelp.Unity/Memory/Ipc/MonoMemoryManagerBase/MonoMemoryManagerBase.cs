using System.Diagnostics;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Unity.Memory.Ipc;

public abstract partial class MonoMemoryManagerBase : MemoryManager, IMonoMemoryManager
{
    public MonoMemoryManagerBase(Process process)
        : this(process, null) { }

    public MonoMemoryManagerBase(Process process, ILogger? logger)
        : base(process, logger)
    {
        foreach (Module module in Modules)
        {
            switch (module.Name)
            {
                case "mono.dll":
                    MonoModule = module;
                    RuntimeVersion = MonoRuntimeVersion.Mono;
                    return;
                case "mono-2.0-bdwgc.dll":
                    MonoModule = module;
                    RuntimeVersion = MonoRuntimeVersion.Mono20;
                    return;
                case "GameAssembly.dll":
                    MonoModule = module;
                    RuntimeVersion = MonoRuntimeVersion.Il2Cpp;
                    return;
            }
        }

        const string msg = "Could not find supported Mono module.";
        ThrowHelper.ThrowInvalidOperationException(msg);
    }

    public Module MonoModule { get; }

    public MonoRuntimeVersion RuntimeVersion { get; }
}
