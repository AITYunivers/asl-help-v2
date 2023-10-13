using System;

using AslHelp.Unity.Memory.MonoInterop;

namespace AslHelp.Unity.Memory;

public class MonoType(
    nuint address,
    IMonoInteroperator mono)
{
    private readonly IMonoInteroperator _mono = mono;

    public nuint Address { get; } = address;

    private MonoClass? _class;
    public MonoClass Class => _class ??= new(_mono.GetTypeClass(Address), _mono);

    private MonoFieldAttribute? _attributes;
    public MonoFieldAttribute Attributes => _attributes ??= _mono.GetTypeAttributes(Address);

    private MonoElementType? _elementType;
    public MonoElementType ElementType => _elementType ??= _mono.GetTypeElementType(Address);
}
