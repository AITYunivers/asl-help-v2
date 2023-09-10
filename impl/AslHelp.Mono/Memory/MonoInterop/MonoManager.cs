using System.Diagnostics.CodeAnalysis;

using AslHelp.Mono.Memory.Ipc;

namespace AslHelp.Mono.Memory.MonoInterop;

internal abstract class MonoManager : IMonoManager
{
    private protected abstract IMonoInitializer Initializer { get; }

    public static IMonoManager Initialize(IMonoMemoryManager memory)
    {

    }

    public abstract MonoImage FindImage(string imageName);
    public abstract bool TryFindImage(string imageName, [NotNullWhen(true)] out MonoImage? monoImage);

    public abstract MonoClass FindClass(string className);
    public abstract MonoClass FindClass(string @namespace, string className);
    public abstract bool TryFindClass(string className, [NotNullWhen(true)] out MonoClass? monoClass);
    public abstract bool TryFindClass(string @namespace, string className, [NotNullWhen(true)] out MonoClass? monoClass);

    public abstract MonoClass GetParentClass(MonoClass monoClass);
    public abstract bool TryGetParentClass(MonoClass monoClass, [NotNullWhen(true)] out MonoClass? parent);
}
