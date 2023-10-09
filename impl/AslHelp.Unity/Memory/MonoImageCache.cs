using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Unity.Memory.MonoInterop;

namespace AslHelp.Unity.Memory;

public class MonoImageCache(
    IMonoManager mono) : LazyDictionary<string, MonoImage>
{
    private readonly IMonoManager _mono = mono;

    public override IEnumerator<MonoImage> GetEnumerator()
    {
        foreach (nuint image in _mono.GetImages())
        {
            yield return new(image, _mono);
        }
    }

    protected override string GetKey(MonoImage value)
    {
        return value.Name;
    }
}
