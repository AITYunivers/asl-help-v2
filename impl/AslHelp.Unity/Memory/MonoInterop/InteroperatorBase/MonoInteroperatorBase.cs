using System;
using System.Collections.Generic;

using AslHelp.Common.Results;
using AslHelp.Core.Collections;
using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;
using AslHelp.Unity.Memory.MonoInterop.Initialization;

namespace AslHelp.Unity.Memory.MonoInterop;

public abstract partial class MonoInteroperatorBase : IMonoInteroperator
{
    protected readonly IMonoMemoryManager _memory;
    protected readonly NativeStructMap _structs;
    protected readonly nuint _loadedAssemblies;
    protected readonly MonoDefaults _defaults;

    protected MonoInteroperatorBase(
        IMonoMemoryManager memory,
        NativeStructMap structs,
        nuint loadedAssemblies,
        nuint defaults)
    {
        _memory = memory;
        _structs = structs;
        _loadedAssemblies = loadedAssemblies;

        // Nothing better we can do here.
        Span<nuint> ptrs = stackalloc nuint[18];
        memory.ReadSpan(ptrs, defaults);

        _defaults = new(
            new(ptrs[0], this),
            new(ptrs[1], this),
            new(ptrs[2], this),
            new(ptrs[3], this),
            new(ptrs[4], this),
            new(ptrs[5], this),
            new(ptrs[6], this),
            new(ptrs[7], this),
            new(ptrs[8], this),
            new(ptrs[9], this),
            new(ptrs[10], this),
            new(ptrs[11], this),
            new(ptrs[12], this),
            new(ptrs[13], this),
            new(ptrs[14], this),
            new(ptrs[15], this),
            new(ptrs[16], this),
            new(ptrs[17], this));

        Images = new MonoImageCache(this);
    }

    public LazyDictionary<string, MonoImage> Images { get; }

    public abstract IEnumerable<nuint> GetImages();
    public abstract string GetImageName(nuint image);
    public abstract string GetImageFileName(nuint image);

    public abstract IEnumerable<nuint> GetImageClasses(nuint image);
    public abstract string GetClassName(nuint klass);
    public abstract string GetClassNamespace(nuint klass);
    public abstract nuint GetClassParent(nuint klass);

    public abstract IEnumerable<nuint> GetClassFields(nuint klass);
    public abstract string GetFieldName(nuint field);
    public abstract int GetFieldOffset(nuint field);
    public abstract nuint GetFieldType(nuint field);

    public abstract nuint GetTypeClass(nuint type);
    public abstract MonoFieldAttribute GetTypeAttributes(nuint type);
    public abstract MonoElementType GetTypeElementType(nuint type);

    public static Result<IMonoInteroperator, MonoInitializationError> Initialize(IMonoMemoryManager memory)
    {
        MonoRuntimeVersion version = memory.RuntimeVersion;

        IMonoInitializer? initializer = version switch
        {
            MonoRuntimeVersion.Mono => new MonoV1Initializer(),
            MonoRuntimeVersion.Mono20 => new MonoV2Initializer(),
            _ => null
        };

        if (initializer == null)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.Unknown);
        }

        var initializeStructsResult = initializer.InitializeStructs(memory);
        if (!initializeStructsResult.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: new(MonoInitializationError.Unknown, initializeStructsResult.Error.Message));
        }

        var findLoadedAssembliesResult = initializer.FindLoadedAssemblies(memory);
        if (!findLoadedAssembliesResult.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.Unknown);
        }

        var findDefaultsResult = initializer.FindDefaults(memory);
        if (!findDefaultsResult.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.Unknown);
        }

#pragma warning disable CS8509 // The switch expression is not exhaustive.
        IMonoInteroperator mono = memory.RuntimeVersion switch
#pragma warning restore CS8509
        {
            MonoRuntimeVersion.Mono => new MonoV1Interoperator(
                memory,
                initializeStructsResult.Value,
                findLoadedAssembliesResult.Value,
                findDefaultsResult.Value),
            MonoRuntimeVersion.Mono20 => new MonoV2Interoperator(
                memory,
                initializeStructsResult.Value,
                findLoadedAssembliesResult.Value,
                findDefaultsResult.Value)
        };

        return new(
            IsSuccess: true,
            Value: mono);
    }
}
