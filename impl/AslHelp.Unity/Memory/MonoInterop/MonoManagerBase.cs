using System.Collections.Generic;

using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public abstract class MonoManagerBase : IMonoManager
{
    protected readonly IMonoMemoryManager _memory;

    protected readonly NativeStructMap _structs;
    protected readonly nuint _assemblies;

    protected MonoManagerBase(IMonoMemoryManager memory)
    {
        _memory = memory;

        _structs = InitializeStructs();
        _assemblies = FindLoadedAssemblies();
    }

    protected abstract NativeStructMap InitializeStructs();
    protected abstract nuint FindLoadedAssemblies();

    public abstract IEnumerable<nuint> GetImages();
    public abstract string GetImageName(nuint image);
    public abstract string GetImageFileName(nuint image);

    public abstract IEnumerable<nuint> GetImageClasses(nuint image);
    public abstract string GetClassName(nuint klass);
    public abstract string GetClassNamespace(nuint klass);

    public abstract IEnumerable<nuint> GetClassFields(nuint klass);
    public abstract string GetFieldName(nuint field);
    public abstract int GetFieldOffset(nuint field);
}
