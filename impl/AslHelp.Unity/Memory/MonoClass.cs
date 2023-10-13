using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Unity.Memory.MonoInterop;

namespace AslHelp.Unity.Memory;

public class MonoClass(
    nuint address,
    IMonoInteroperator mono)
{
    private readonly IMonoInteroperator _mono = mono;

    public nuint Address { get; } = address;

    private string? _name;
    public string Name => _name ??= _mono.GetClassName(Address);

    private string? _namespace;
    public string Namespace => _namespace ??= _mono.GetClassNamespace(Address);

    public LazyDictionary<string, MonoField> Fields { get; } = new MonoFieldCache(address, mono);

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

    private class MonoFieldCache(
        nuint address,
        IMonoInteroperator mono) : LazyDictionary<string, MonoField>
    {
        private readonly nuint _address = address;
        private readonly IMonoInteroperator _mono = mono;

        public override IEnumerator<MonoField> GetEnumerator()
        {
            foreach (nuint field in _mono.GetClassFields(_address))
            {
                yield return new(field, _mono);
            }
        }

        protected override string GetKey(MonoField value)
        {
            return value.Name;
        }
    }
}
