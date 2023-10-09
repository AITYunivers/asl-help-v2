using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Collections;
using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public abstract class MonoManagerBase : IMonoManager
{
    protected readonly IMonoMemoryManager _memory;

    protected MonoManagerBase(IMonoMemoryManager memory)
    {
        _memory = memory;

        Images = new MonoImageCache(this);
    }

    public LazyDictionary<string, MonoImage> Images { get; }

    protected NativeStructMap Structs { get; private set; }
    protected nuint LoadedAssemblies { get; private set; }

    protected abstract bool TryInitializeStructs([NotNullWhen(true)] out NativeStructMap? structs);
    protected abstract bool TryFindLoadedAssemblies(out nuint loadedAssemblies);

    public static bool TryInitializeMono(IMonoMemoryManager memory, [NotNullWhen(true)] out IMonoManager? mono)
    {
        if (memory.MonoModule.Name == "mono.dll")
        {
            mono = new MonoV1Manager(memory);
        }
        else if (memory.MonoModule.Name == "mono-2.0-bdwgc.dll")
        {
            mono = new MonoV2Manager(memory);
        }
        else
        {
            mono = null;
            return false;
        }

        return TryInitialize((MonoManagerBase)mono);
    }

    public static bool TryInitializeIl2Cpp(IMonoMemoryManager memory, int il2CppVersion, [NotNullWhen(true)] out IMonoManager? mono)
    {
        if (memory.MonoModule.Name != "GameAssembly.dll")
        {
            mono = null;
            return false;
        }

        if (il2CppVersion != 24)
        {
            mono = null;
            return false;
        }

        mono = new Il2CppV24Manager(memory);
        return TryInitialize((MonoManagerBase)mono);
    }

    public abstract IEnumerable<nuint> GetImages();
    public abstract string GetImageName(nuint image);
    public abstract string GetImageFileName(nuint image);

    public abstract IEnumerable<nuint> GetImageClasses(nuint image);
    public abstract string GetClassName(nuint klass);
    public abstract string GetClassNamespace(nuint klass);

    public abstract nuint GetClassParent(nuint klass);
    public abstract nuint GetArrayClass(nuint arrayClass);
    public abstract nuint GetGenericInstClass(nuint genericClass);

    public abstract IEnumerable<nuint> GetClassFields(nuint klass);
    public abstract string GetFieldName(nuint field);
    public abstract int GetFieldOffset(nuint field);
    public abstract nuint GetFieldType(nuint field);

    public abstract nuint GetTypeData(nuint type);
    public abstract MonoFieldAttribute GetTypeAttributes(nuint type);
    public abstract MonoElementType GetTypeElementType(nuint type);

    private static bool TryInitialize(MonoManagerBase mono)
    {
        if (mono.TryInitializeStructs(out NativeStructMap? structs)
            && mono.TryFindLoadedAssemblies(out nuint loadedAssemblies))
        {
            mono.Structs = structs;
            mono.LoadedAssemblies = loadedAssemblies;

            return true;
        }

        return false;
    }
}
