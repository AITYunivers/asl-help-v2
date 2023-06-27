using System.Collections.Generic;

using AslHelp.Common.Exceptions;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.Memory.Pointers;

public partial class Basic
{
    private readonly Dictionary<string, IPointer?> _pointerCache = new();
    public IPointer? this[string name]
    {
        get => _pointerCache[name];
        set => _pointerCache[name] = value;
    }

    private PointerFactory? _pointers;
    public PointerFactory? Pointers
    {
        get
        {
            string action = Actions.CurrentAction;
            if (action is "startup" or "exit" or "shutdown")
            {
                string msg = $"Attempted to access the pointer factory in the '{action}' action.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            return _pointers;
        }
    }

    public void MapPointerValuesToCurrent()
    {
        foreach (KeyValuePair<string, IPointer?> pair in _pointerCache)
        {
            Script.Current[pair.Key] = pair.Value?.Current;
        }
    }
}
