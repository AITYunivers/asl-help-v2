using AslHelp.Common.Results;
using AslHelp.Unity.Memory.Ipc;
using AslHelp.Unity.Memory.MonoInterop.Initialization;

namespace AslHelp.Unity.Memory.MonoInterop;

public abstract partial class Il2CppManagerBase : MonoManagerBase
{
    protected readonly nuint _typeInfoDefinitions;

    protected Il2CppManagerBase(IMonoMemoryManager memory,
        IIl2CppInitializer initializer,
        out Result<IMonoManager, MonoInitializationError> result)
        : base(memory, initializer, out result)
    {
        var typeInfoDefinitionsResult = initializer.FindTypeInfoDefinitions(memory);
        if (!typeInfoDefinitionsResult.IsSuccess)
        {
            result = new(
                IsSuccess: false,
                Error: typeInfoDefinitionsResult.Error);

            return;
        }

        _typeInfoDefinitions = typeInfoDefinitionsResult.Value;
    }
}
