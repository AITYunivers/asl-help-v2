using AslHelp.Core.Exceptions;
using AslHelp.Core.Helping.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;

namespace AslHelp.Core.Helping.Asl;

public abstract partial class AslHelperBase
{
    private string _gameName;

    string IAslHelper.GameName => GameName;
    protected string GameName
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => _gameName ?? Game?.ProcessName ?? "Auto Splitter";
    }

    IAslHelper.InitStage IAslHelper.InitStage.GameName(string gameName)
    {
        ThrowHelper.ThrowIfNullOrEmpty(gameName);

        _gameName = gameName;

        return this;
    }

    Process IAslHelper.Game
    {
        get
        {
            string action = Actions.CurrentAction;
            if (action is "startup" or "exit" or "shutdown")
            {
                string msg = $"Attempted to access game process in the '{action}' action.";
                ThrowHelper.Throw.InvalidOperation(msg);
            }

            return Game;
        }
        set
        {
            string action = Actions.CurrentAction;
            if (action is "startup" or "exit" or "shutdown")
            {
                string msg = $"Attempted to set game process in the '{action}' action.";
                ThrowHelper.Throw.InvalidOperation(msg);
            }

            Game = value;
        }
    }

    protected abstract Process Game
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set;
    }
}
