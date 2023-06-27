using System.Diagnostics;

using AslHelp.Common.Exceptions;
using AslHelp.Core.LiveSplitInterop;

public partial class Basic
{
    private string? _gameName;
    public string GameName
    {
        get => _gameName ?? Game?.ProcessName ?? "Auto Splitter";
        set => _gameName = value;
    }

    public Basic SetGameName(string gameName)
    {
        ThrowHelper.ThrowIfNullOrEmpty(gameName);

        GameName = gameName;

        return this;
    }

    private Process? _game;
    public Process? Game
    {
        get
        {
            string action = Actions.CurrentAction;
            if (action is "startup" or "exit" or "shutdown")
            {
                string msg = $"Attempted to access game process in the '{action}' action.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            _game ??= Script.Game;

            return _game;
        }
        set
        {
            string action = Actions.CurrentAction;
            if (action is "startup" or "exit" or "shutdown")
            {
                string msg = $"Attempted to set game process in the '{action}' action.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            Script.Game = value;
            _game = value;
        }
    }
}
