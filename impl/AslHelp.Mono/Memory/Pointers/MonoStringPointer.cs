using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Memory.Pointers;
using AslHelp.Mono.Memory.Ipc;

namespace AslHelp.Mono.Memory.Pointers;

public sealed class MonoStringPointer<T> : PointerBase<string>
{
    private readonly new IMonoMemoryManager _manager;

    public MonoStringPointer(IMonoMemoryManager manager, nuint baseAddress, params int[] offsets)
        : base(manager, baseAddress, offsets)
    {
        _manager = manager;
    }

    public MonoStringPointer(IMonoMemoryManager manager, IPointer<nuint> parent, int nextOffset, params int[] remainingOffsets)
        : base(manager, parent, nextOffset, remainingOffsets)
    {
        _manager = manager;
    }

    protected override string? Default { get; } = null;

    protected override bool TryUpdate([NotNullWhen(true)] out string? result, nuint address)
    {
        return _manager.TryReadString(out result, address);
    }

    protected override bool Write(string value, nuint address)
    {
        throw new NotImplementedException();
    }

    protected override bool HasChanged(string? old, string? current)
    {
        return old != current;
    }

    public override string ToString()
    {
        return $"{nameof(StringPointer)}({OffsetsToString()})";
    }
}
