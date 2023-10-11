
using System.Collections.Generic;

using LiveSplit.ComponentUtil;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class Il2CppV24Manager
{
    public override IEnumerable<nuint> GetClassFields(nuint klass)
    {
        nuint fields = Il2CppClassFields(klass);

        if (fields == 0)
        {
            yield break;
        }

        uint fieldCount = Il2CppClassFieldCount(klass);

        for (uint i = 0; i < fieldCount; i++)
        {
            yield return fields + (i * Structs["FieldInfo"].SelfAlignedSize);
        }
    }

    public override string GetFieldName(nuint field)
    {
        nuint nameStart = _memory.Read<nuint>(field + Structs["FieldInfo"]["name"]);
        return _memory.ReadString(128, ReadStringType.UTF8, nameStart);
    }

    public override int GetFieldOffset(nuint field)
    {
        return _memory.Read<int>(field + Structs["FieldInfo"]["offset"]);
    }

    public override nuint GetFieldType(nuint field)
    {
        return _memory.Read<nuint>(field + Structs["FieldInfo"]["type"]);
    }
}
