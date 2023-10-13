using System.Collections.Generic;

using AslHelp.Common.Results;

using LiveSplit.ComponentUtil;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV1Interoperator
{
    public override IEnumerable<nuint> GetClassFields(nuint klass)
    {
        nuint fields = MonoClassFields(klass);

        if (fields == 0)
        {
            yield break;
        }

        uint fieldCount = MonoClassFieldCount(klass);

        for (uint i = 0; i < fieldCount; i++)
        {
            yield return fields + (i * _structs["MonoClassField"].SelfAlignedSize);
        }
    }

    public override string GetFieldName(nuint field)
    {
        nuint nameStart = _memory.Read<nuint>(field + _structs["MonoClassField"]["name"]);
        return _memory.ReadString(128, ReadStringType.UTF8, nameStart);
    }

    public override int GetFieldOffset(nuint field)
    {
        return _memory.Read<int>(field + _structs["MonoClassField"]["offset"]);
    }

    public override nuint GetFieldType(nuint field)
    {
        return _memory.Read<nuint>(field + _structs["MonoClassField"]["type"]);
    }
}
