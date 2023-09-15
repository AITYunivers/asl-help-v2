using System.Diagnostics.CodeAnalysis;

namespace AslHelp.Mono.Memory.MonoInterop;

public interface IMonoManager
{
    MonoImage FindImage(string imageName);
    bool TryFindImage(string imageName, [NotNullWhen(true)] out MonoImage? monoImage);

    MonoClass FindClass(nuint image, string className);
    MonoClass FindClass(nuint image, string @namespace, string className);

    bool TryFindClass(nuint image, string className, [NotNullWhen(true)] out MonoClass? monoClass);
    bool TryFindClass(nuint image, string @namespace, string className, [NotNullWhen(true)] out MonoClass? monoClass);

    // MonoClass GetParentClass(MonoClass monoClass);
    // bool TryGetParentClass(MonoClass monoClass, [NotNullWhen(true)] out MonoClass? parent);
}

public record MonoImage
{
    private readonly IMonoManager _manager;

    public MonoImage(IMonoManager manager, nuint address, string name, string path)
    {
        _manager = manager;

        Address = address;
        Name = name;
        Path = path;
    }

    public nuint Address { get; }
    public string Name { get; }
    public string Path { get; }

    public MonoClass FindClass(string className)
    {
        return _manager.FindClass(Address, className);
    }

    public MonoClass FindClass(string @namespace, string className)
    {
        return _manager.FindClass(Address, @namespace, className);
    }

    public override string ToString()
    {
        return $"{Name} {{ Address: 0x{(ulong)Address:X}, Path: {Path} }}";
    }
}

public record MonoClass(
    nuint Address,
    string Name,
    string Namespace);

public record MonoField;
public record MonoType;
