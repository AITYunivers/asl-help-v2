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
            EnsureInitialized();

            return _game ??= Script.Game;
        }
        set
        {
            _game = value;
            Script.Game = value;
        }
    }
}
