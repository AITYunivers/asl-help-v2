using AslHelp.Core.Helping;
using AslHelp.Core.Reflection;

public partial class Basic
    : IAslHelper
{
    public Basic()
        : this(true) { }

    public Basic(bool generateCode)
    {
        InitForAsl();

        if (generateCode)
        {
            GenerateCode();
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;

        TypeDefinitionFactory.Dispose();
        _logger.DisposeFileLoggers();
        _logger = null;

        bool closing = Debug.Trace.Any(
            "TimerForm.TimerForm_FormClosing",
            "TimerForm.OpenLayoutFromFile",
            "TimerForm.LoadDefaultLayout");

        if (!closing)
        {
            Texts.RemoveAll();
        }
    }
}
