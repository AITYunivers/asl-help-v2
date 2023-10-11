using System;
using System.Collections.Generic;

using LiveSplit.ComponentUtil;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class Il2CppV24Manager
{
    public override IEnumerable<nuint> GetImageClasses(nuint image)
    {
        nuint typeInfos = _memory.Read<nuint>(_typeInfoDefinitionTable);

        int typeStart = Il2CppImageTypeStart(image);
        uint typeCount = Il2CppImageTypeCount(image);

        nuint[] types = _memory.ReadSpan<nuint>((int)typeCount, typeInfos + (_memory.PointerSize * (uint)typeStart));
        foreach (nuint type in types)
        {
            yield return type;
        }
    }

    public override string GetClassName(nuint klass)
    {
        nuint nameStart = _memory.Read<nuint>(klass + Structs["Il2CppClass"]["name"]);
        return _memory.ReadString(128, ReadStringType.UTF8, nameStart);
    }

    public override string GetClassNamespace(nuint klass)
    {
        nuint nameStart = _memory.Read<nuint>(klass + Structs["Il2CppClass"]["namespaze"]);
        return _memory.ReadString(256, ReadStringType.UTF8, nameStart);
    }

    public override nuint GetClassParent(nuint klass)
    {
        return _memory.Read<nuint>(klass + Structs["Il2CppClass"]["parent"]);
    }
}
