using System.Collections.Generic;

using AslHelp.Core.Collections;
using AslHelp.Core.Memory.Pointers;
using AslHelp.Mono.Memory.Pointers.Initialization;

using OneOf;

namespace AslHelp.Mono;

public class MonoImage : LazyDictionary<string, MonoClass>, IPointerFromImageFactory
{
    public Pointer<T> Make<T>(string className, string staticFieldName, params OneOf<string, int>[] next) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator<MonoClass> GetEnumerator()
    {
        throw new System.NotImplementedException();
    }

    protected override string GetKey(MonoClass value)
    {
        return value.Name;
    }
}
