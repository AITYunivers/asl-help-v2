using AslHelp.Core.IO.Parsing;
using AslHelp.Mono.Memory.Ipc;
using AslHelp.Mono.Memory.MonoInterop.MonoV1;

namespace AslHelp.Mono.Memory.MonoInterop;

public abstract partial class MonoManager : IMonoManager
{
    protected readonly IMonoMemoryManager _memory;

    protected readonly NativeStructMap _structs;
    protected readonly nuint _loadedAssemblies;

    protected MonoManager(IMonoMemoryManager memory, IMonoInitializer initializer)
    {
        _memory = memory;

        _structs = initializer.InitializeStructs(memory.Is64Bit);
        _loadedAssemblies = initializer.InitializeAssemblies(memory);
    }

    public static IMonoManager Initialize(IMonoMemoryManager memory)
    {
        return new MonoV1Manager(memory);
    }
}
