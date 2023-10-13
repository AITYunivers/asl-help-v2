
using System;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class Il2CppV24Interoperator
{
    public override nuint GetTypeClass(nuint type)
    {
        throw new NotImplementedException();
    }

    public override MonoFieldAttribute GetTypeAttributes(nuint type)
    {
        return _memory.Read<MonoFieldAttribute>(type + _structs["Il2CppType"]["attrs"]);
    }

    public override MonoElementType GetTypeElementType(nuint type)
    {
        return _memory.Read<MonoElementType>(type + _structs["Il2CppType"]["type"]);
    }
}
