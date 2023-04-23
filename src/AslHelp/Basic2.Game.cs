public partial class Basic
{
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
