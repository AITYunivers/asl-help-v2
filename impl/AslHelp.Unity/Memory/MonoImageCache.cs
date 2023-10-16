using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Unity.Memory.MonoInterop.Management;

namespace AslHelp.Unity.Memory;

internal class MonoImageCache(
    MonoManager mono) : LazyDictionary<string, MonoImage>
{
    private readonly MonoManager _mono = mono;

    public override IEnumerator<MonoImage> GetEnumerator()
    {
        foreach (nuint image in _mono.GetImages())
        {
            if (!_mono.ImageCache.TryGetValue(image, out MonoImage? monoImage))
            {
                monoImage = new(image, _mono);
                _mono.ImageCache.Add(image, monoImage);
            }

            yield return monoImage;
        }
    }

    protected override string GetKey(MonoImage value)
    {
        return value.Name;
    }

    protected override string KeyNotFoundMessage(string key)
    {
        return $"An image with the given name '{key}' was not present.";
    }
}
