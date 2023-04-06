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
            if (_game is null)
            {
                _game = Script.Game;
                InitMemory();
            }

            return _game;
        }
        set
        {
            _game = value;
            Script.Game = value;

            InitMemory();
        }
    }
}
