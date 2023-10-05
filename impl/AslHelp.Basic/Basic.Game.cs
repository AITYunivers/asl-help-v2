using System.Diagnostics;

using AslHelp.Common.Exceptions;
using AslHelp.Core.LiveSplitInterop;

public partial class Basic
{
    private Process? _game;
    public override Process? Game
    {
        get
        {
            _game ??= Script.Game;
            return _game;
        }
        set
        {
            string action = Actions.CurrentAction;
            if (action is not "init")
            {
                string msg = $"Attempted to set game process in the '{action}' action.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            _game = value;
            Script.Game = value;

            if (value is null)
            {
                DisposeProcessInstanceData();
            }
        }
    }
}
