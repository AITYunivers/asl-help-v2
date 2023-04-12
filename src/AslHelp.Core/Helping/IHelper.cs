using AslHelp.Core.IO.Logging;
using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Helping;

public interface IHelper
{
    Process Game { get; set; }
    IMemoryManager Memory { get; }

    ILogger Logger { get; }
}
