using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Core.Memory.Pointers;
using AslHelp.Unity.Memory.MonoInterop;
using AslHelp.Unity.Memory.Pointers.Initialization;

using OneOf;

namespace AslHelp.Unity;

public class MonoClass(nuint address, IMonoManager mono) : IPointerFromClassFactory
{
    private readonly IMonoManager _mono = mono;

    public nuint Address { get; } = address;

    private string? _name;
    public string Name => _name ??= _mono.GetClassName(Address);

    private string? _namespace;
    public string Namespace => _namespace ??= _mono.GetClassNamespace(Address);

    public LazyDictionary<string, MonoField> Fields { get; } = new MonoFieldCache(address, mono);

    public Pointer<T> Make<T>(string staticFieldName, params OneOf<string, int>[] next) where T : unmanaged
    {
        throw new System.NotImplementedException();
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
