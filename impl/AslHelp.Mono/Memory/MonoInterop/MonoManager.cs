using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Diagnostics;
using AslHelp.Core.IO.Parsing;
using AslHelp.Mono.Memory.Ipc;
using AslHelp.Mono.Memory.MonoInterop.MonoV1;

namespace AslHelp.Mono.Memory.MonoInterop;

public abstract class MonoManager : IMonoManager
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

    public abstract IEnumerable<nuint> EnumerateImages();
    public abstract IEnumerable<nuint> EnumerateClasses(nuint image);
    public abstract IEnumerable<nuint> EnumerateFields(nuint klass);

    public abstract string GetImageName(nuint image);

    public abstract string GetClassName(nuint klass);
}
