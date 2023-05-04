using AslHelp.Core.Memory.IO;
using AslHelp.Core.Reflection;

namespace AslHelp.Core.Memory.Pointers;

public sealed partial class TypeDefinitionPointer
    : PointerBase<dynamic>
{
    private readonly ITypeDefinition _definition;

    public TypeDefinitionPointer(IMemoryManager manager, ITypeDefinition definition, nint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets)
    {
        _definition = definition;
        Default = definition.Default;
    }

    public TypeDefinitionPointer(IMemoryManager manager, ITypeDefinition definition, IPointer<nint> parent, int nextOffset, params int[] offsets)
        : base(manager, parent, nextOffset, offsets)
    {
        _definition = definition;
        Default = definition.Default;
    }

    protected override dynamic Default { get; }

    protected override bool TryUpdate(out dynamic result)
    {
        return _manager.TryReadDef(_definition, out result, DerefOffsets());
    }

    protected override bool CheckChanged(dynamic old, dynamic current)
    {
        return !old.Equals(current);
    }

    protected override bool Write(dynamic value)
    {
        return _manager.Write(value, DerefOffsets());
    }

    public override string ToString()
    {
        return $"TypeDefinitionPointer({OffsetsToString()})";
    }
}
