using AslHelp.Common.Results;
using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop.Initialization;

public interface IMonoInitializer
{
    Result<NativeStructMap, MonoInitializationError> InitializeStructs(IMonoMemoryManager memory);
    Result<nuint, MonoInitializationError> FindLoadedAssemblies(IMonoMemoryManager memory);
    Result<nuint, MonoInitializationError> FindDefaults(IMonoMemoryManager memory);
}
