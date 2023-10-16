using System.Collections.Generic;

using LiveSplit.ComponentUtil;

namespace AslHelp.Unity.Memory.MonoInterop.Management;

public partial class MonoV1Manager
{
    public override IEnumerable<nuint> GetImageClasses(nuint image)
    {
        nuint classCache = image + _structs["MonoImage"]["class_cache"];

        int classCacheSize = _memory.Read<int>(classCache + _structs["MonoInternalHashTable"]["size"]);
        nuint classCacheTable = _memory.Read<nuint>(classCache + _structs["MonoInternalHashTable"]["table"]);

        nuint[] classes = _memory.ReadSpan<nuint>(classCacheSize, classCacheTable);

        for (int i = 0; i < classes.Length; i++)
        {
            for (nuint klass = classes[i]; klass != 0; klass = MonoClassNextClassCache(klass))
            {
                yield return klass;
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

    public override nuint GetClassParent(nuint klass)
    {
        return _memory.Read<nuint>(klass + _structs["MonoClass"]["parent"]);
    }

    public override nuint GetClassStaticDataChunk(nuint klass)
    {
        return _memory.Read<nuint>(MonoClassVTable(klass) + _structs["MonoVTable"]["data"]);
    }
}
