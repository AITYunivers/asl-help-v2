using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.IO.Parsing;
using AslHelp.Mono.Memory.Ipc;
using AslHelp.Mono.Memory.MonoInterop.MonoV1;

namespace AslHelp.Mono.Memory.MonoInterop;

internal abstract class MonoManager : IMonoManager
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

    protected abstract IEnumerable<nuint> EnumerateImages();
    protected abstract IEnumerable<nuint> EnumerateClasses(nuint image);
    protected abstract IEnumerable<nuint> EnumerateFields(nuint klass);
}
