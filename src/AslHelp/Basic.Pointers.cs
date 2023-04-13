using AslHelp.Core.Exceptions;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.Memory.Pointers;

public partial class Basic
{
    private PointerFactory _pointers;
    public PointerFactory Pointers
    {
        get
        {
            EnsureInitialized();

            if (Methods.CurrentMethod is "startup" or "exit" or "shutdown")
            {
                string msg = "Attempted to access the pointer factory while the game is not running.";
                ThrowHelper.Throw.InvalidOperation(msg);
            }

            return _pointers ??= new(Memory);
        }
    }

    private readonly Dictionary<string, IPointer> _ptrCache = new();
    public IPointer this[string name]
    {
        get => _ptrCache[name];
        set => _ptrCache[name] = value;
    }

    public void MapPointerValuesToCurrent()
    {
        foreach (KeyValuePair<string, IPointer> entry in _ptrCache)
        {
            Script.Current[entry.Key] = entry.Value.Current;
        }
    }
}
