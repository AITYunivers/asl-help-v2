using System.Collections.Generic;

using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.Memory.Pointers;
using AslHelp.Core.Memory.Pointers.Initialization;

public partial class Basic
{
    private readonly Dictionary<string, IPointer> _pointerCache = new();

    public sealed override IPointer this[string name]
    {
        get => _pointerCache[name];
        set => _pointerCache[name] = value;
    }

    protected IPointerFactory? _pointers;
    public override IPointerFactory? Pointers => _pointers;

    public sealed override void MapPointerValuesToCurrent()
    {
        foreach (var ptr in _pointerCache)
        {
            Script.Current[ptr.Key] = ptr.Value.Current;
        }
    }
}
