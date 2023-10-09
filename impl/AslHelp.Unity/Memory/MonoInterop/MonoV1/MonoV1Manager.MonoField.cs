using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.IO.Parsing;

using LiveSplit.ComponentUtil;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV1Manager
{
    public override IEnumerable<nuint> GetClassFields(nuint klass)
    {
        for (; klass != 0; klass = GetClassParent(klass))
        {
            nuint fields = MonoClassFields(klass);

            if (fields == 0)
            {
                continue;
            }

            uint fieldCount = MonoClassFieldCount(klass);

            for (uint i = 0; i < fieldCount; i++)
            {
                yield return fields + (i * Structs["MonoClassField"].SelfAlignedSize);
            }
        }
    }

    public override string GetFieldName(nuint field)
    {
        nuint nameStart = _memory.Read<nuint>(field + Structs["MonoClassField"]["name"]);
        return _memory.ReadString(128, ReadStringType.UTF8, nameStart);
    }

    public override int GetFieldOffset(nuint field)
    {
        return _memory.Read<int>(field + Structs["MonoClassField"]["offset"]);
    }

    public override nuint GetFieldType(nuint field)
    {
        return _memory.Read<nuint>(field + Structs["MonoClassField"]["type"]);
    }
}
