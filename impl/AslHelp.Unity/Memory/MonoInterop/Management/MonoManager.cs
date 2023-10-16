using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using AslHelp.Common.Results;
using AslHelp.Core.Collections;
using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;
using AslHelp.Unity.Memory.MonoInterop.Initialization;

namespace AslHelp.Unity.Memory.MonoInterop.Management;

public abstract partial class MonoManager
{
    protected internal readonly IMonoMemoryManager _memory;
    protected readonly NativeStructMap _structs;
    protected readonly nuint _loadedAssemblies;
    protected readonly MonoDefaults _defaults;

    internal Dictionary<nuint, MonoImage> ImageCache { get; } = new();
    internal Dictionary<nuint, MonoClass> ClassCache { get; } = new();
    internal Dictionary<nuint, MonoField> FieldCache { get; } = new();

    protected MonoManager(
        IMonoMemoryManager memory,
        NativeStructMap structs,
        nuint loadedAssemblies,
        nuint defaults)
    {
        _memory = memory;
        _structs = structs;
        _loadedAssemblies = loadedAssemblies;

        Span<nuint> ptrs = stackalloc nuint[18];
        memory.ReadSpan(ptrs, defaults);

        _defaults = Unsafe.As<nuint, MonoDefaults>(ref ptrs[0]);

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
    public abstract nuint GetClassStaticDataChunk(nuint klass);

    public abstract IEnumerable<nuint> GetClassFields(nuint klass);
    public abstract string GetFieldName(nuint field);
    public abstract int GetFieldOffset(nuint field);
    public abstract nuint GetFieldType(nuint field);

    public abstract nuint GetTypeClass(nuint type);
    public abstract MonoFieldAttribute GetTypeAttributes(nuint type);
    public abstract MonoElementType GetTypeElementType(nuint type);

    public static Result<MonoManager, MonoInitializationError> Initialize(IMonoMemoryManager memory)
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
                Error: MonoInitializationError.UnsupportedMonoModule);
        }

        var initializeStructsResult = initializer.InitializeStructs(memory);
        if (!initializeStructsResult.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: initializeStructsResult.Error);
        }

        var findLoadedAssembliesResult = initializer.FindLoadedAssemblies(memory);
        if (!findLoadedAssembliesResult.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: findLoadedAssembliesResult.Error);
        }

        var findDefaultsResult = initializer.FindDefaults(memory);
        if (!findDefaultsResult.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: findDefaultsResult.Error);
        }

#pragma warning disable CS8509 // The switch expression is not exhaustive.
        MonoManager mono = version switch
#pragma warning restore CS8509
        {
            MonoRuntimeVersion.Mono => new MonoV1Manager(
                memory,
                initializeStructsResult.Value,
                findLoadedAssembliesResult.Value,
                findDefaultsResult.Value),
            MonoRuntimeVersion.Mono20 => new MonoV2Manager(
                memory,
                initializeStructsResult.Value,
                findLoadedAssembliesResult.Value,
                findDefaultsResult.Value)
        };

        return new(
            IsSuccess: true,
            Value: mono);
    }

    public static Result<MonoManager, MonoInitializationError> Initialize(IMonoMemoryManager memory, int il2CppVersion)
    {
        MonoRuntimeVersion version = memory.RuntimeVersion;

        IIl2CppInitializer? initializer = (version, il2CppVersion) switch
        {
            (MonoRuntimeVersion.Il2Cpp, <= 24) => new Il2CppV24Initializer(),
            _ => null
        };

        if (initializer == null)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.UnsupportedIl2CppVersion);
        }

        var initializeStructsResult = initializer.InitializeStructs(memory);
        if (!initializeStructsResult.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: initializeStructsResult.Error);
        }

        var findLoadedAssembliesResult = initializer.FindLoadedAssemblies(memory);
        if (!findLoadedAssembliesResult.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: findLoadedAssembliesResult.Error);
        }

        var findTypeInfoDefinitionsResult = initializer.FindTypeInfoDefinitions(memory);
        if (!findTypeInfoDefinitionsResult.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: findTypeInfoDefinitionsResult.Error);
        }

        var findDefaultsResult = initializer.FindDefaults(memory);
        if (!findDefaultsResult.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: findDefaultsResult.Error);
        }

#pragma warning disable CS8509 // The switch expression is not exhaustive.
        MonoManager mono = (version, il2CppVersion) switch
#pragma warning restore CS8509
        {
            (MonoRuntimeVersion.Il2Cpp, <= 24) => new Il2CppV24Manager(
                memory,
                initializeStructsResult.Value,
                findLoadedAssembliesResult.Value,
                findTypeInfoDefinitionsResult.Value,
                findDefaultsResult.Value)
        };

        return new(
            IsSuccess: true,
            Value: mono);
    }
}
