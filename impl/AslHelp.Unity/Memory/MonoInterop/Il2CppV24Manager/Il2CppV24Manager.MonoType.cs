
using System;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class Il2CppV24Manager
{
    public override nuint GetTypeClass(nuint type)
    {
        throw new NotImplementedException();
    }

    public override MonoFieldAttribute GetTypeAttributes(nuint type)
    {
        return _memory.Read<MonoFieldAttribute>(type + Structs["Il2CppType"]["attrs"]);
    }

    public override MonoElementType GetTypeElementType(nuint type)
    {
        return _memory.Read<MonoElementType>(type + Structs["Il2CppType"]["type"]);
    }
}
