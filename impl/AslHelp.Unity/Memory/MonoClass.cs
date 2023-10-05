using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Unity.Memory.MonoInterop;

namespace AslHelp.Unity.Memory;

public class MonoClass
{
    private readonly IMonoManager _mono;

    public MonoClass(nuint address, IMonoManager mono)
    {
        _mono = mono;

        Address = address;
        Fields = new MonoFieldCache(address, mono);
    }

    public nuint Address { get; }

    private string? _name;
    public string Name => _name ??= _mono.GetClassName(Address);

    private string? _namespace;
    public string Namespace => _namespace ??= _mono.GetClassNamespace(Address);

    public LazyDictionary<string, MonoField> Fields { get; }

    public override string ToString()
    {
        if (string.IsNullOrEmpty(Namespace))
        {
            return Name;
        }
        else
        {
            return $"{Namespace}.{Name}";
        }
    }

    private class MonoFieldCache(nuint address, IMonoManager mono) : LazyDictionary<string, MonoField>
    {
        private readonly nuint _address = address;
        private readonly IMonoManager _mono = mono;

        public override IEnumerator<MonoField> GetEnumerator()
        {
            foreach (nuint field in _mono.GetClassFields(_address))
            {
                yield return new(field, _mono);
            }
        }

        protected override string GetKey(MonoField value)
        {
            return $"{value.Name}";
        }
    }
}
