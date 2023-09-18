using System.Collections.Generic;

using AslHelp.Core.Diagnostics;
using AslHelp.Core.IO.Parsing;
using AslHelp.Mono.Memory.Ipc;
using AslHelp.Mono.Memory.MonoInterop.MonoV1;
using AslHelp.Mono.Memory.MonoInterop.MonoV2;

namespace AslHelp.Mono.Memory.MonoInterop;

internal abstract partial class MonoManager : IMonoManager
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
        if (memory.MonoModule.Name == "mono.dll")
        {
            return new MonoV1Manager(memory);
        }
        else
        {
            return new MonoV2Manager(memory);
        }
    }

    public abstract IEnumerable<nuint> GetImages();
    public abstract string GetImageName(nuint image);
    public abstract string GetImagePath(nuint image);

    public abstract IEnumerable<nuint> GetClasses(nuint image);
    public abstract string GetClassName(nuint klass);
    public abstract string GetClassNamespace(nuint klass);

    public abstract IEnumerable<nuint> GetFields(nuint klass);
    public abstract string GetFieldName(nuint field);
    public abstract int GetFieldOffset(nuint field);
}
