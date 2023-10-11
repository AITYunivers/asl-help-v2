using System;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV1Manager
{
    public override nuint GetTypeClass(nuint type)
    {
        throw new NotImplementedException();
    }

    public override MonoFieldAttribute GetTypeAttributes(nuint type)
    {
        return _memory.Read<MonoFieldAttribute>(type + Structs["MonoType"]["attrs"]);
    }

    public override MonoElementType GetTypeElementType(nuint type)
    {
        return _memory.Read<MonoElementType>(type + Structs["MonoType"]["type"]);
    }
}
