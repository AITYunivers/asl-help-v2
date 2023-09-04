using System.Diagnostics.CodeAnalysis;

namespace AslHelp.Mono.MonoInterop;

public interface IMonoManager
{
    MonoClass FindClass(string className);
    MonoClass FindClass(string @namespace, string className);

    bool TryFindClass(string className, [NotNullWhen(true)] out MonoClass? monoClass);
    bool TryFindClass(string @namespace, string className, [NotNullWhen(true)] out MonoClass? monoClass);

    MonoClass GetParentClass(MonoClass monoClass);
    bool TryGetParentClass(MonoClass monoClass, [NotNullWhen(true)] out MonoClass? parent);
}

public record MonoImage();
public record MonoClass();
public record MonoField();
public record MonoType();
