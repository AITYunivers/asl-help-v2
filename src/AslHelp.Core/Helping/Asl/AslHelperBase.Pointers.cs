using AslHelp.Core.Exceptions;
using AslHelp.Core.Helping.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.Memory.Pointers;

namespace AslHelp.Core.Helping.Asl;

public abstract partial class AslHelperBase
{
    protected abstract IPointer this[string name]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set;
    }

    IPointer IAslHelper.this[string name]
    {
        get => this[name];
        set => this[name] = value;
    }

    protected abstract PointerFactory Pointers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
    }

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

            return Pointers;
        }
    }

    protected abstract void MapPointerValuesToCurrent();

    IAslHelper IAslHelper.MapPointerValuesToCurrent()
    {
        MapPointerValuesToCurrent();

        return this;
    }
}
