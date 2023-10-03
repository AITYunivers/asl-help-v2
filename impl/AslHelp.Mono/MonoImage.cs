using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Mono.Memory.MonoInterop;

namespace AslHelp.Mono;

public class MonoImage : LazyDictionary<string, MonoClass>
{
    private readonly IMonoManager _manager;

    public override IEnumerator<MonoClass> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    protected override string GetKey(MonoClass value)
    {
        throw new System.NotImplementedException();
    }
}
