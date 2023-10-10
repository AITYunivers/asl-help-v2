using System.Diagnostics;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Unity.Memory.Ipc;

public abstract partial class MonoMemoryManagerBase : MemoryManagerBase, IMonoMemoryManager
{
    public MonoMemoryManagerBase(Process process)
        : this(process, null) { }

    public MonoMemoryManagerBase(Process process, ILogger? logger)
        : base(process, logger)
    {
        foreach (Module module in Modules)
        {
            string name = module.Name;
            if (name is "mono.dll" or "mono-2.0-bdwgc.dll")
            {
                MonoModule = module;
                return;
            }
            else if (name is "GameAssembly.dll")
            {
                IsIl2Cpp = true;
                MonoModule = module;
                return;
            }
        }

        const string msg = "Could not find supported Mono module.";
        ThrowHelper.ThrowInvalidOperationException(msg);
    }

    public Module MonoModule { get; }

    public bool IsIl2Cpp { get; }
}
