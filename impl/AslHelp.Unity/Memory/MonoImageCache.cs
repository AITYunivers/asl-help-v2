using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Unity.Memory.MonoInterop;

namespace AslHelp.Unity.Memory;

public class MonoImageCache(
    IMonoInteroperator mono) : LazyDictionary<string, MonoImage>
{
    private readonly IMonoInteroperator _mono = mono;

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
