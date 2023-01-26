using AslHelp.Core.Reflection;

public partial class Basic
{
    internal static Basic Instance { get; private set; }

    public Basic()
        : this(true) { }

    public Basic(bool generateCode)
    {
        Instance = this;

        Init(generateCode);
    }

    private string _gameName;
    public string GameName
    {
        get => _gameName ?? Game?.ProcessName ?? "Auto Splitter";
        set => _gameName = value;
    }

    public void Dispose()
    {
        Dispose(true);
    }

    public virtual void Dispose(bool removeTexts)
    {
        TypeDefinitionFactory.Dispose();
        _fileLogger?.Dispose();
        _fileLogger = null;

        bool closing = Debug.Trace.Any(
            "TimerForm.TimerForm_FormClosing",
            "TimerForm.OpenLayoutFromFile",
            "TimerForm.LoadDefaultLayout");

        if (!closing && removeTexts)
        {
            //Texts.RemoveAll();
        }
    }
}
