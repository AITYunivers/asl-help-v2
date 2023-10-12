using System.Collections.Generic;

using AslHelp.Common.Results;
using AslHelp.Core.Collections;
using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;
using AslHelp.Unity.Memory.MonoInterop.Initialization;

namespace AslHelp.Unity.Memory.MonoInterop;

public abstract partial class MonoManagerBase : IMonoManager
{
#nullable disable
    protected readonly IMonoMemoryManager _memory;
    protected readonly NativeStructMap _structs;
    protected readonly nuint _loadedAssemblies;
    protected readonly nuint[] _defaults;
#nullable restore

    protected MonoManagerBase(
        IMonoMemoryManager memory,
        IMonoInitializer initializer,
        out Result<MonoInitializationError> result)
    {
        var initializeStructsResult = initializer.InitializeStructs(memory);
        if (!initializeStructsResult.IsSuccess)
        {
            result = new(
                IsSuccess: false,
                Error: MonoInitializationError.Unknown);

            return;
        }

        var findLoadedAssembliesResult = initializer.FindLoadedAssemblies(memory);
        if (!findLoadedAssembliesResult.IsSuccess)
        {
            result = new(
                IsSuccess: false,
                Error: MonoInitializationError.Unknown);

            return;
        }

        var findDefaultsResult = initializer.FindDefaults(memory);
        if (!findDefaultsResult.IsSuccess)
        {
            result = new(
                IsSuccess: false,
                Error: MonoInitializationError.Unknown);

            return;
        }

        _memory = memory;
        _structs = initializeStructsResult.Value;
        _loadedAssemblies = findLoadedAssembliesResult.Value;
        _defaults = findDefaultsResult.Value;

        Images = new MonoImageCache(this);

        result = new(
            IsSuccess: true);
    }

#nullable disable
    public LazyDictionary<string, MonoImage> Images { get; }
#nullable restore

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
}
