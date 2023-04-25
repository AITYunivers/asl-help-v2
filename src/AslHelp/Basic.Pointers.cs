using AslHelp.Core.Memory.Pointers;

public partial class Basic
{
    private readonly Dictionary<string, IPointer> _pointerCache = new();
    protected override IPointer this[string name]
    {
        get => _pointerCache[name];
        set => _pointerCache[name] = value;
    }

    private PointerFactory _pointers;
    protected override PointerFactory Pointers => _pointers;

    protected override void MapPointerValuesToCurrent()
    {
        foreach (KeyValuePair<string, IPointer> pair in _pointerCache)
        {
            Script.Current[pair.Key] = pair.Value.Current;
        }
    }
}
