using System;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV1Interoperator
{
    public override nuint GetTypeClass(nuint type)
    {
        MonoElementType elementType = GetTypeElementType(type);

        switch (elementType)
        {
            case MonoElementType.Object:
            {
                return _defaults.ObjectClass;
            }
            case MonoElementType.Void:
            {
                return _defaults.VoidClass;
            }
            case MonoElementType.Boolean:
            {
                return _defaults.BooleanClass;
            }
            case MonoElementType.Char:
            {
                return _defaults.CharClass;
            }
            case MonoElementType.I1:
            {
                return _defaults.SByteClass;
            }
            case MonoElementType.U1:
            {
                return _defaults.ByteClass;
            }
            case MonoElementType.I2:
            {
                return _defaults.Int16Class;
            }

        }

        throw new NotImplementedException();
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
