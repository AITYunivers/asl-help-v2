using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public abstract partial class MonoManagerBase : IMonoManager
{
    protected readonly IMonoMemoryManager _memory;

    protected MonoManagerBase(IMonoMemoryManager memory)
    {
        _memory = memory;

        Images = new MonoImageCache(this);
    }

    public LazyDictionary<string, MonoImage> Images { get; }

#nullable disable
    protected NativeStructMap Structs { get; private set; }
    protected nuint LoadedAssemblies { get; private set; }
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
