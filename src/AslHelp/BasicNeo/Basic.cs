using AslHelp.Core.Helping;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.Reflection;

namespace AslHelp.Neo;

public partial class Basic
    : IAslHelper
{
    public Basic()
    {
        InitForAsl();
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
