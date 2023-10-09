using System.Diagnostics;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Unity.Memory.Ipc;
using AslHelp.Unity.Memory.MonoInterop;

public partial class Unity
{
    protected override void DisposeProcessInstanceData()
    {
        base.DisposeProcessInstanceData();

        _mono = null;

        _il2CppMetadata = null;
        _unityVersion = null;
    }

    private IMonoManager? _mono;
    public IMonoManager? Mono
    {
        get
        {
            if (_mono is not null)
            {
                return _mono;
            }

            if (Memory is IMonoMemoryManager memory)
            {
                Debug.Info("Initializing Mono...");

                _mono = InitializeMono(memory);

                Debug.Info("  => Done.");
            }

            return _mono;
        }
    }

    protected override IMemoryManager InitializeMemory(Process process)
    {
        return new MonoExternalMemoryManager(process, Logger);
    }

    private IMonoManager InitializeMono(IMonoMemoryManager memory)
    {
        if (memory.MonoModule.Name == "mono.dll")
        {
            return new MonoV1Manager(memory);
        }
        else if (memory.MonoModule.Name == "mono-2.0-bdwgc.dll")
        {
            return new MonoV2Manager(memory);
        }
        else if (memory.MonoModule.Name != "GameAssembly.dll")
        {
            const string msg =
                "Target process is not a Mono or Unity game: " +
                "'mono.dll', 'mono-2.0-bdwgc.dll', or 'GameAssembly.dll' not found.";

            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        int? version = Il2CppMetadata?.Version;
        if (version is null or < 24 or > 29)
        {
            const string msg = "This version of IL2CPP is not supported.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return version switch
        {
            <= 24 => new Il2CppV24Manager(memory),
            _ => throw new()
            // <= 27 => new Il2CppV27Manager(memory),
            // <= 29 => new Il2CppV29Manager(memory)
        };
    }
}
