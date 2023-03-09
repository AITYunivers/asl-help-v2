using AslHelp.Core.Helping;
using AslHelp.Core.Memory.Pointers;
using AslHelp.Core.Reflection;

public partial class Basic
    : IAslHelper
{
    public Basic()
        : this(true) { }

    public Basic(bool generateCode)
    {
        Init();

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

        for (int i = 0; i < _files.Count; i++)
        {
            _files[i].Dispose();
        }

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
