using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Core.Memory.Pointers;
using AslHelp.Unity.Memory.MonoInterop;
using AslHelp.Unity.Memory.Pointers.Initialization;

using OneOf;

namespace AslHelp.Unity;

public class MonoImage(nuint address, IMonoManager mono) : LazyDictionary<string, MonoClass>, IPointerFromImageFactory
{
    private readonly IMonoManager _mono = mono;

    public nuint Address { get; } = address;

    private string? _name;
    public string Name => _name ??= _mono.GetImageName(Address);

    private string? _fileName;
    public string FileName => _fileName ??= _mono.GetImageFileName(Address);

    public Pointer<T> Make<T>(string className, string staticFieldName, params OneOf<string, int>[] next) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

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
