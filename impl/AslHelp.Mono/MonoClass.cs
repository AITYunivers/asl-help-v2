using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Mono.Memory.MonoInterop;

namespace AslHelp.Mono;

public class MonoClass
{
    private readonly IMonoManager _manager;

    public MonoClass(IMonoManager manager)
    {
        _manager = manager;
        Fields = new MonoFieldCache(_manager);
    }

    public LazyDictionary<string, MonoField> Fields { get; }
}

internal class MonoFieldCache : LazyDictionary<string, MonoField>
{
    private readonly IMonoManager _manager;

    public MonoFieldCache(IMonoManager manager)
    {
        _manager = manager;
    }

    public override IEnumerator<MonoField> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    protected override string GetKey(MonoField value)
    {
        throw new System.NotImplementedException();
    }
}
