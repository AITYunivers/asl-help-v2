using System.Collections.Generic;

using AslHelp.Core.Collections;

namespace AslHelp.Core.IO.Parsing;

public sealed class NativeStruct : OrderedDictionary<string, NativeField>
{
    public IEnumerable<NativeField> Fields => this;

    public required string Name { get; init; }
    public required string? Super { get; init; }

    public uint Size => this[^1].Offset + this[^1].Size;
    public uint Alignment => this[0].Alignment;
    public uint SelfAlignedSize => NativeFieldParser.Align(Size, Alignment);

    protected override string GetKeyForItem(NativeField item)
    {
        return item.Name;
    }
}
