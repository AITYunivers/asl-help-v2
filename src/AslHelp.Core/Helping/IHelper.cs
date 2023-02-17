using AslHelp.Core.IO.Logging;
using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Helping;

public interface IHelper
{
    string GameName { get; set; }
    Process Game { get; set; }
    IMemoryManager Memory { get; }

    LoggerBase Logger { get; }

    void Log(object output);

    void Dispose();
}
