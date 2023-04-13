using AslHelp.Core.IO.Logging;
using AslHelp.Core.Memory.IO;
using AslHelp.Core.Memory.Pointers;

namespace AslHelp.Core.Helping;

public interface IHelper
{
    Process Game { get; set; }
    IMemoryManager Memory { get; }
    PointerFactory Pointers { get; }

    ILogger Logger { get; }
}
