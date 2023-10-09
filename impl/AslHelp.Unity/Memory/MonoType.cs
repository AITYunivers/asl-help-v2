using System;

using AslHelp.Unity.Memory.MonoInterop;

namespace AslHelp.Unity.Memory;

public class MonoType(
    nuint address,
    IMonoManager mono)
{
    private readonly IMonoManager _mono = mono;

    public nuint Address { get; } = address;

    private nuint? _data;
    public nuint Data => _data ??= _mono.GetTypeData(Address);

    private MonoFieldAttribute? _attributes;
    public MonoFieldAttribute Attributes => _attributes ??= _mono.GetTypeAttributes(Address);

    private MonoElementType? _elementType;
    public MonoElementType ElementType => _elementType ??= _mono.GetTypeElementType(Address);

    private MonoClass? _class;
    public MonoClass Class
    {
        get
        {
            if (_class is not null)
            {
                return _class;
            }

            nuint data = Data;
            MonoElementType type = ElementType;

            _class = type switch
            {
                // `data` is `nullptr` for primitive types
                MonoElementType.Void => SystemVoid,
                MonoElementType.Boolean => SystemBoolean,
                MonoElementType.I => SystemIntPtr,
                MonoElementType.U => SystemUIntPtr,
                MonoElementType.Char => SystemChar,
                MonoElementType.I1 => SystemSByte,
                MonoElementType.U1 => SystemByte,
                MonoElementType.I2 => SystemInt16,
                MonoElementType.U2 => SystemUInt16,
                MonoElementType.I4 => SystemInt32,
                MonoElementType.U4 => SystemUInt32,
                MonoElementType.I8 => SystemInt64,
                MonoElementType.U8 => SystemUInt64,
                MonoElementType.R4 => SystemSingle,
                MonoElementType.R8 => SystemDouble,
                MonoElementType.String => SystemString,
                MonoElementType.Object => SystemObject,

                MonoElementType.Ptr => new MonoType(data, _mono).Class,
                MonoElementType.Class or MonoElementType.ValueType => new(data, _mono),
                MonoElementType.Array or MonoElementType.SzArray => new(_mono.GetArrayClass(data), _mono),
                MonoElementType.GenericInst => new(_mono.GetGenericInstClass(data), _mono),
                _ => throw new NotSupportedException($"Creating a MonoClass from type '{type}' is not supported.")
            };

            return _class;
        }
    }

    private MonoClass? _systemVoid;
    private MonoClass SystemVoid => _systemVoid ??= _mono.Images["mscorlib"]["System.Void"];

    private MonoClass? _systemBoolean;
    private MonoClass SystemBoolean => _systemBoolean ??= _mono.Images["mscorlib"]["System.Boolean"];

    private MonoClass? _systemIntPtr;
    private MonoClass SystemIntPtr => _systemIntPtr ??= _mono.Images["mscorlib"]["System.IntPtr"];

    private MonoClass? _systemUIntPtr;
    private MonoClass SystemUIntPtr => _systemUIntPtr ??= _mono.Images["mscorlib"]["System.UIntPtr"];

    private MonoClass? _systemChar;
    private MonoClass SystemChar => _systemChar ??= _mono.Images["mscorlib"]["System.Char"];

    private MonoClass? _systemSByte;
    private MonoClass SystemSByte => _systemSByte ??= _mono.Images["mscorlib"]["System.SByte"];

    private MonoClass? _systemByte;
    private MonoClass SystemByte => _systemByte ??= _mono.Images["mscorlib"]["System.Byte"];

    private MonoClass? _systemInt16;
    private MonoClass SystemInt16 => _systemInt16 ??= _mono.Images["mscorlib"]["System.Int16"];

    private MonoClass? _systemUInt16;
    private MonoClass SystemUInt16 => _systemUInt16 ??= _mono.Images["mscorlib"]["System.UInt16"];

    private MonoClass? _systemInt32;
    private MonoClass SystemInt32 => _systemInt32 ??= _mono.Images["mscorlib"]["System.Int32"];

    private MonoClass? _systemUInt32;
    private MonoClass SystemUInt32 => _systemUInt32 ??= _mono.Images["mscorlib"]["System.UInt32"];

    private MonoClass? _systemInt64;
    private MonoClass SystemInt64 => _systemInt64 ??= _mono.Images["mscorlib"]["System.Int64"];

    private MonoClass? _systemUInt64;
    private MonoClass SystemUInt64 => _systemUInt64 ??= _mono.Images["mscorlib"]["System.UInt64"];

    private MonoClass? _systemSingle;
    private MonoClass SystemSingle => _systemSingle ??= _mono.Images["mscorlib"]["System.Single"];

    private MonoClass? _systemDouble;
    private MonoClass SystemDouble => _systemDouble ??= _mono.Images["mscorlib"]["System.Double"];

    private MonoClass? _systemString;
    private MonoClass SystemString => _systemString ??= _mono.Images["mscorlib"]["System.String"];

    private MonoClass? _systemObject;
    private MonoClass SystemObject => _systemObject ??= _mono.Images["mscorlib"]["System.Object"];
}
