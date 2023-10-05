namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV1Manager
{
    public override nuint GetTypeData(nuint type)
    {
        return _memory.Read<nuint>(type + _structs["MonoType"]["data"]);
    }

    public override MonoFieldAttribute GetTypeAttributes(nuint type)
    {
        return _memory.Read<MonoFieldAttribute>(type + _structs["MonoType"]["attrs"]);
    }

    public override MonoElementType GetTypeElementType(nuint type)
    {
        return _memory.Read<MonoElementType>(type + _structs["MonoType"]["type"]);
    }
}
