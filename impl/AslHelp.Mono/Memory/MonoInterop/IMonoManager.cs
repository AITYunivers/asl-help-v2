using System.Collections.Generic;

using AslHelp.Core.Collections;

namespace AslHelp.Mono.Memory.MonoInterop;

internal interface IMonoManager
{
    IEnumerable<nuint> GetImages();
    IEnumerable<nuint> GetClasses(nuint image);
    IEnumerable<nuint> GetFields(nuint klass);

    string GetImageName(nuint image);
    string GetImagePath(nuint image);

    string GetClassName(nuint klass);
    string GetClassNamespace(nuint klass);

    string GetFieldName(nuint field);
    int GetFieldOffset(nuint field);
}

internal class MonoEngine : LazyDictionary<string, MonoImage>, IEngine
{
    private readonly IMonoManager _manager;

    public MonoEngine(IMonoManager manager)
    {
        _manager = manager;
    }

    public IEnumerable<IModule> GetModules()
    {
        return this;
    }

    public override IEnumerator<MonoImage> GetEnumerator()
    {
        foreach (nuint image in _manager.GetImages())
        {
            yield return new(_manager, image);
        }
    }

    protected override string GetKey(MonoImage value)
    {
        return value.Name;
    }
}

internal class MonoImage : LazyDictionary<string, MonoClass>, IModule
{
    private readonly IMonoManager _manager;

    public MonoImage(IMonoManager manager, nuint address)
    {
        _manager = manager;

        Address = address;
    }

    public nuint Address { get; }

    private string? _name;
    public string Name => _name ??= _manager.GetImageName(Address);

    private string? _fileName;
    public string FileName => _fileName ??= _manager.GetImagePath(Address);

    public IEnumerable<IType> GetTypes()
    {
        return this;
    }

    public override IEnumerator<MonoClass> GetEnumerator()
    {
        foreach (nuint klass in _manager.GetClasses(Address))
        {
            yield return new(_manager, klass);
        }
    }

    protected override string GetKey(MonoClass value)
    {
        string name = value.Name, ns = value.Namespace;
        if (string.IsNullOrEmpty(ns))
        {
            return name;
        }
        else
        {
            return $"{ns}.{name}";
        }
    }
}

internal interface IEngine
{
    IEnumerable<IModule> GetModules();
}

internal interface IModule
{
    nuint Address { get; }
    string Name { get; }

    IEnumerable<IType> GetTypes();
}

internal interface IType
{
    nuint Address { get; }
    string Name { get; }
    string Namespace { get; }

    IEnumerable<IMember> GetMembers();
}

internal interface IMember
{
    nuint Address { get; }
    string Name { get; }
    int Offset { get; }
}

internal class MonoClass : LazyDictionary<string, MonoField>, IType
{
    private readonly IMonoManager _manager;

    public MonoClass(IMonoManager manager, nuint address)
    {
        _manager = manager;

        Address = address;
    }

    public nuint Address { get; }

    private string? _name;
    public string Name => _name ??= _manager.GetClassName(Address);

    private string? _namespace;
    public string Namespace => _namespace ??= _manager.GetClassNamespace(Address);

    public IEnumerable<IMember> GetMembers()
    {
        return this;
    }

    public override IEnumerator<MonoField> GetEnumerator()
    {
        foreach (nuint field in _manager.GetFields(Address))
        {
            yield return new(_manager, field);
        }
    }

    protected override string GetKey(MonoField value)
    {
        return value.Name;
    }
}

internal class MonoField : IMember
{
    private readonly IMonoManager _manager;

    public MonoField(IMonoManager manager, nuint address)
    {
        _manager = manager;
        Address = address;
    }

    public nuint Address { get; }

    private string? _name;
    public string Name => _name ??= _manager.GetFieldName(Address);

    private int? _offset;
    public int Offset => _offset ??= _manager.GetFieldOffset(Address);
}

public record MonoType;
