using System;

using AslHelp.Unity.Memory.MonoInterop;

namespace AslHelp.Unity.Memory;

public class MonoType
{
    private readonly IMonoManager _mono;

    public MonoType(nuint address, IMonoManager mono)
    {
        _mono = mono;

        Address = address;
    }

    public nuint Address { get; }

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
                MonoElementType.Ptr => new MonoType(data, _mono).Class,
                MonoElementType.ValueType or MonoElementType.Class => new(data, _mono),
                MonoElementType.Array or MonoElementType.SzArray => new(_mono.GetArrayClass(data), _mono),
                MonoElementType.GenericInst => new(_mono.GetGenericInstClass(data), _mono),
                _ => new(data, _mono)
            };

            return _class;
        }
    }
}
