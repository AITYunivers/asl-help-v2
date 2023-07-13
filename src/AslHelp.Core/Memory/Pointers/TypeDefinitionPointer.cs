using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Reflection;

namespace AslHelp.Core.Memory.Pointers;

public sealed class TypeDefinitionPointer : PointerBase<dynamic>
{
    private readonly ITypeDefinition _typeDefinition;

    public TypeDefinitionPointer(IMemoryManager manager, ITypeDefinition typeDefinition, nuint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets)
    {
        _typeDefinition = typeDefinition;

        Default = typeDefinition.Default;
    }

    public TypeDefinitionPointer(
        IMemoryManager manager,
        ITypeDefinition typeDefinition,
        IPointer<nuint> parent,
        int nextOffset,
        params int[] remainingOffsets)
        : base(manager, parent, nextOffset, remainingOffsets)
    {
        _typeDefinition = typeDefinition;

        Default = typeDefinition.Default;
    }

    protected override dynamic? Default { get; }

    protected override bool TryUpdate([NotNullWhen(true)] out dynamic? result, nuint address)
    {
        return _manager.TryReadDef(_typeDefinition, out result, address);
    }

    protected override bool Write(dynamic value, nuint address)
    {
        throw new NotImplementedException();
    }

    protected override bool HasChanged(dynamic? old, dynamic? current)
    {
        return old != current;
    }

    public override string ToString()
    {
        return $"{nameof(TypeDefinitionPointer)}<{typeof(string).Name}>({OffsetsToString()})";
    }
}
