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

    private IMonoInteroperator? _mono;
    public IMonoInteroperator? Mono
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

                var initializationResult = MonoInteroperatorBase.Initialize(memory);
                if (!initializationResult.IsSuccess)
                {
                    Debug.Error($"Failed to initialize Mono ({initializationResult.Error}).");
                }
                else
                {
                    _mono = initializationResult.Value;
                    Debug.Info("  => Done.");
                }
            }

            return _mono;
        }
    }

    protected override IMemoryManager InitializeMemory(Process process)
    {
        return new ExternalMonoMemoryManager(process, Logger);
    }
}
