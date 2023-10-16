using System.Diagnostics;

using AslHelp.Common.Results;
using AslHelp.Core.Collections;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Unity.Memory;
using AslHelp.Unity.Memory.Ipc;
using AslHelp.Unity.Memory.MonoInterop;
using AslHelp.Unity.Memory.MonoInterop.Management;

public partial class Unity
{
    protected override void DisposeProcessInstanceData()
    {
        base.DisposeProcessInstanceData();

        _mono = null;

        _il2CppMetadata = null;
        _unityVersion = null;
    }

    private MonoManager? _mono;
    public MonoManager? Mono
    {
        get
        {
            if (_mono is not null)
            {
                return _mono;
            }

            if (Memory is not IMonoMemoryManager memory)
            {
                return null;
            }

            Debug.Info("Initializing Mono...");

            Result<MonoManager, MonoInitializationError> initializationResult;

            if (memory.RuntimeVersion == MonoRuntimeVersion.Il2Cpp)
            {
                if (Il2CppMetadata is not { Version: int il2CppVersion })
                {
                    Debug.Error("  => Failed: Il2CppMetadata is null.");
                    return null;
                }

                Debug.Info($"  => Il2Cpp version: {il2CppVersion}");
                initializationResult = MonoManager.Initialize(memory, il2CppVersion);
            }
            else
            {
                initializationResult = MonoManager.Initialize(memory);
            }

            if (!initializationResult.IsSuccess)
            {
                Debug.Error($"=> Failed: '{initializationResult.Error}'");
            }
            else
            {
                _mono = initializationResult.Value;
                Debug.Info("  => Done.");
            }

            return _mono;
        }
    }

    private LazyDictionary<string, MonoImage>? _images;
    public LazyDictionary<string, MonoImage>? Images
    {
        get
        {
            if (_images is not null)
            {
                return _images;
            }

            if (Mono is not null)
            {
                _images = new MonoImageCache(Mono);
            }

            return _images;
        }
    }

    protected override IMemoryManager InitializeMemory(Process process)
    {
        return new ExternalMonoMemoryManager(process, Logger);
    }
}
