using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Unity.Memory.MonoInterop.Management;

namespace AslHelp.Unity.Memory;

internal class MonoClassCache(
    nuint image,
    MonoManager mono) : LazyDictionary<string, MonoClass>
{
    private readonly nuint _image = image;
    private readonly MonoManager _mono = mono;

    public override IEnumerator<MonoClass> GetEnumerator()
    {
        foreach (nuint klass in _mono.GetImageClasses(_image))
        {
            if (!_mono.ClassCache.TryGetValue(klass, out MonoClass? monoClass))
            {
                monoClass = new(klass, _mono);
                _mono.ClassCache.Add(klass, monoClass);
            }

            yield return monoClass;
        }
    }

    protected override string GetKey(MonoClass value)
    {
        return value.Name;
    }

    protected override string KeyNotFoundMessage(string key)
    {
        return $"A class with the given name '{key}' was not present in the image '{_mono.GetImageName(_image)}'.";
    }
}
