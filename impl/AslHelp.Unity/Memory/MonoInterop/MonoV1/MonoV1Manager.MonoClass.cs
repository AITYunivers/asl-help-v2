using System.Collections.Generic;

using LiveSplit.ComponentUtil;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV1Manager
{
    public override IEnumerable<nuint> GetImageClasses(nuint image)
    {
        nuint classCache = image + _structs["MonoImage"]["class_cache"];

        int classCacheSize = _memory.Read<int>(classCache + _structs["MonoInternalHashTable"]["size"]);
        nuint classCacheTable = _memory.Read<nuint>(classCache + _structs["MonoInternalHashTable"]["table"]);

        nuint[] classes = new nuint[classCacheSize];
        if (!_memory.TryReadSpan<nuint>(classes, classCacheTable))
        {
            yield break;
        }

        for (int i = 0; i < classes.Length; i++)
        {
            nuint klass = classes[i];
            while (klass != 0)
            {
                yield return klass;
                klass = MonoClassNextClassCache(klass);
            }
        }
    }

    public override string GetClassName(nuint klass)
    {
        nuint nameStart = _memory.Read<nuint>(klass + _structs["MonoClass"]["name"]);
        return _memory.ReadString(128, ReadStringType.UTF8, nameStart);
    }

    public override string GetClassNamespace(nuint klass)
    {
        nuint nameSpaceStart = _memory.Read<nuint>(klass + _structs["MonoClass"]["name_space"]);
        return _memory.ReadString(256, ReadStringType.UTF8, nameSpaceStart);
    }
}
