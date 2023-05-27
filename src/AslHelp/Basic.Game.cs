using System.Diagnostics;

public partial class Basic
{
    private string _gameName;
    protected override string GameName
    {
        get => _gameName ?? Game?.ProcessName ?? "Auto Splitter";
        set => _gameName = value;
    }

    private Process _game;
    protected override Process Game
    {
        get
        {
            _game ??= Script.Game;

            return _game;
        }
        set
        {
            Script.Game = value;
            _game = value;
        }
    }
}
