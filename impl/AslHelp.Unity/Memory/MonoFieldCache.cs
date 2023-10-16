using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Unity.Memory.MonoInterop.Management;

namespace AslHelp.Unity.Memory;

internal class MonoFieldCache(
    nuint klass,
    MonoManager mono) : LazyDictionary<string, MonoField>
{
    private readonly nuint _klass = klass;
    private readonly MonoManager _mono = mono;

    public override IEnumerator<MonoField> GetEnumerator()
    {
        foreach (nuint klass in _mono.GetClassFields(_klass))
        {
            if (!_mono.FieldCache.TryGetValue(klass, out MonoField? monoField))
            {
                monoField = new(klass, _mono);
                _mono.FieldCache.Add(klass, monoField);
            }

            yield return monoField;
        }
    }

    protected override string GetKey(MonoField value)
    {
        return value.Name;
    }

    protected override string KeyNotFoundMessage(string key)
    {
        return $"A field with the given name '{key}' was not present in the class '{_mono.GetClassName(_klass)}'.";
    }
}
