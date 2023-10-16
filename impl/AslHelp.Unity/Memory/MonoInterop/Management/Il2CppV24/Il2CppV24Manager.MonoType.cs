
using System;

namespace AslHelp.Unity.Memory.MonoInterop.Management;

public partial class Il2CppV24Manager
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
            case MonoElementType.U2:
            {
                return _defaults.UInt16Class;
            }
            case MonoElementType.I4:
            {
                return _defaults.Int32Class;
            }
            case MonoElementType.U4:
            {
                return _defaults.UInt32Class;
            }
            case MonoElementType.I8:
            {
                return _defaults.Int64Class;
            }
            case MonoElementType.U8:
            {
                return _defaults.UInt64Class;
            }
            case MonoElementType.R4:
            {
                return _defaults.SingleClass;
            }
            case MonoElementType.R8:
            {
                return _defaults.DoubleClass;
            }
            case MonoElementType.String:
            {
                return _defaults.StringClass;
            }
            case MonoElementType.I:
            {
                return _defaults.IntPtrClass;
            }
            case MonoElementType.U:
            {
                return _defaults.UIntPtrClass;
            }
            // case MonoElementType.Array:
            // {
            //     return MonoArrayTypeClass(MonoTypeData(type));
            // }
            // case MonoElementType.SzArray:
            // {
            //     return MonoTypeData(type);
            // }
            case MonoElementType.Class:
            case MonoElementType.ValueType:
            {
                uint klassIndex = (uint)Il2CppTypeData(type);
                nuint typeInfoDefinitionsTable = _memory.Read<nuint>(_typeInfoDefinitions);

                return _memory.Read<nuint>(typeInfoDefinitionsTable + (_memory.PointerSize * klassIndex));
            }
            // case MonoElementType.GenericInst:
            // {
            //     return MonoGenericClassClass(MonoTypeData(type));
            // }
            // case MonoElementType.Ptr:
            // {
            //     return GetTypeClass(MonoTypeData(type));
            // }
        }

        string msg = $"Getting MonoClass for type {elementType} is not implemented.";
        throw new NotImplementedException(msg);
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
