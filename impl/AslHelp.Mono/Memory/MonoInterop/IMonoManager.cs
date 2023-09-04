using System.Diagnostics.CodeAnalysis;

using AslHelp.Mono.Memory.Ipc;

namespace AslHelp.Mono.MonoInterop;

public interface IMonoManager
{
    MonoImage GetImage(string imageName);
    bool TryGetImage(string imageName, [NotNullWhen(true)] out MonoImage? monoImage);

    MonoClass FindClass(string className);
    MonoClass FindClass(string @namespace, string className);

    bool TryFindClass(string className, [NotNullWhen(true)] out MonoClass? monoClass);
    bool TryFindClass(string @namespace, string className, [NotNullWhen(true)] out MonoClass? monoClass);

    MonoClass GetParentClass(MonoClass monoClass);
    bool TryGetParentClass(MonoClass monoClass, [NotNullWhen(true)] out MonoClass? parent);
}

public record MonoImage;
public record MonoClass;
public record MonoField;
public record MonoType;
