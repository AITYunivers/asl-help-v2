using AslHelp.Common.Results;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop.Initialization;

public interface IIl2CppInitializer : IMonoInitializer
{
    Result<nuint, MonoInitializationError> FindTypeInfoDefinitions(IMonoMemoryManager memory);
}
