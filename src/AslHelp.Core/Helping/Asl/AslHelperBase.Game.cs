using System.Diagnostics;
using System.Runtime.CompilerServices;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Helping.Asl.Contracts;
using AslHelp.Core.LiveSplitInterop;

namespace AslHelp.Core.Helping.Asl;

public abstract partial class AslHelperBase
{
    string IAslHelper.GameName => GameName;
    protected abstract string GameName
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set;
    }

    IAslHelper.InitStage IAslHelper.InitStage.GameName(string gameName)
    {
        ThrowHelper.ThrowIfNullOrEmpty(gameName);

        GameName = gameName;

        return this;
    }

    protected abstract Process? Game
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set;
    }

    Process? IAslHelper.Game
    {
        get
        {
            string action = Actions.CurrentAction;
            if (action is "startup" or "exit" or "shutdown")
            {
                string msg = $"Attempted to access game process in the '{action}' action.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            return Game;
        }
        set
        {
            string action = Actions.CurrentAction;
            if (action is "startup" or "exit" or "shutdown")
            {
                string msg = $"Attempted to set game process in the '{action}' action.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            Game = value;
        }
    }
}
