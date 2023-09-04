using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.IO.Parsing;
using AslHelp.Mono.Memory.Ipc;

namespace AslHelp.Mono.MonoInterop.MonoV1;

public abstract class MonoManagerBase : IMonoManager
{
    protected IMonoMemoryManager _memory;
    protected NativeStructMap _structs;

    public MonoManagerBase(IMonoMemoryManager memory)
    {
        _memory = memory;
        _structs = InitializeStructData();
    }

    protected abstract NativeStructMap InitializeStructData();

    public abstract MonoClass FindClass(string className);
    public abstract MonoClass FindClass(string @namespace, string className);
    public abstract MonoImage GetImage(string imageName);
    public abstract MonoClass GetParentClass(MonoClass monoClass);
    public abstract bool TryFindClass(string className, [NotNullWhen(true)] out MonoClass? monoClass);
    public abstract bool TryFindClass(string @namespace, string className, [NotNullWhen(true)] out MonoClass? monoClass);
    public abstract bool TryGetImage(string imageName, [NotNullWhen(true)] out MonoImage? monoImage);
    public abstract bool TryGetParentClass(MonoClass monoClass, [NotNullWhen(true)] out MonoClass? parent);
}

public class MonoV1Manager : MonoManagerBase
{
    public MonoV1Manager(IMonoMemoryManager memory)
        : base(memory) { }

    protected override NativeStructMap InitializeStructData()
    {
        return NativeStructMap.Parse("Mono", "mono", "v1", _memory.Is64Bit);
    }

    public override MonoClass FindClass(string className)
    {
        throw new System.NotImplementedException();
    }

    public override MonoClass FindClass(string @namespace, string className)
    {
        throw new System.NotImplementedException();
    }

    public override MonoImage GetImage(string imageName)
    {
        throw new System.NotImplementedException();
    }

    public override MonoClass GetParentClass(MonoClass monoClass)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryFindClass(string className, [NotNullWhen(true)] out MonoClass? monoClass)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryFindClass(string @namespace, string className, [NotNullWhen(true)] out MonoClass? monoClass)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryGetImage(string imageName, [NotNullWhen(true)] out MonoImage? monoImage)
    {
        throw new System.NotImplementedException();
    }

    public override bool TryGetParentClass(MonoClass monoClass, [NotNullWhen(true)] out MonoClass? parent)
    {
        throw new System.NotImplementedException();
    }
}
