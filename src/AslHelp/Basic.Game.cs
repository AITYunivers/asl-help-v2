public partial class Basic
{
    private string _gameName;
    public string GameName
    {
        get => _gameName ?? Game?.ProcessName ?? "Auto Splitter";
        set => _gameName = value;
    }

    private Process _game;
    public Process Game
    {
        get
        {
            _game ??= Script.Game;

            return _game;
        }
        set
        {
            _game = value;
            Script.Game = value;
        }
    }
}
