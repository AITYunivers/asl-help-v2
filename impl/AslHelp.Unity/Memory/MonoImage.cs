using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Unity.Memory.MonoInterop;

namespace AslHelp.Unity.Memory;

public class MonoImage : LazyDictionary<string, MonoClass>
{
    private readonly IMonoManager _mono;

    public MonoImage(nuint address, IMonoManager mono)
    {
        _mono = mono;

        Address = address;
    }

    public nuint Address { get; }

    private string? _name;
    public string Name => _name ??= _mono.GetImageName(Address);

    private string? _fileName;
    public string FileName => _fileName ??= _mono.GetImageFileName(Address);

    public override IEnumerator<MonoClass> GetEnumerator()
    {
        foreach (nuint klass in _mono.GetImageClasses(Address))
        {
            yield return new(klass, _mono);
        }
    }

    protected override string GetKey(MonoClass value)
    {
        if (string.IsNullOrEmpty(value.Namespace))
        {
            return $"{value.Name}";
        }
        else
        {
            return $"{value.Namespace}.{value.Name}";
        }
    }
}
