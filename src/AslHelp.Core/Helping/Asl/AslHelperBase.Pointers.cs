using AslHelp.Core.Exceptions;
using AslHelp.Core.Helping.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.Memory.Pointers;

namespace AslHelp.Core.Helping.Asl;

public abstract partial class AslHelperBase
{
    private readonly Dictionary<string, IPointer> _ptrCache = new();
    IPointer IAslHelper.this[string name]
    {
        get => _ptrCache[name];
        set => _ptrCache[name] = value;
    }

    protected PointerFactory _pointers;
    PointerFactory IAslHelper.Pointers
    {
        get
        {
            string action = Actions.CurrentAction;
            if (action is "startup" or "exit" or "shutdown")
            {
                string msg = $"Attempted to access the pointer factory in the '{action}' action.";
                ThrowHelper.Throw.InvalidOperation(msg);
            }

            return _pointers ??= new(Memory);
        }
    }

    IAslHelper IAslHelper.MapPointerValuesToCurrent()
    {
        foreach (KeyValuePair<string, IPointer> entry in _ptrCache)
        {
            Script.Current[entry.Key] = entry.Value.Current;
        }

        return this;
    }
}
