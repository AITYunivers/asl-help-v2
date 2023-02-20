using AslHelp.Core.Collections;
using AslHelp.Core.LiveSplitInterop.Texts;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.Memory.Models;

namespace AslHelp.Core.Helping;

public interface IAslHelper
    : IHelper
{
    string GameName { get; set; }

    bool Is64Bit { get; }
    byte PtrSize { get; }

    Module MainModule { get; }
    ModuleCache Modules { get; }

    IEnumerable<MemoryPage> Pages(bool allPages);

    TimerController Timer { get; }
    TextComponentController Texts { get; }
    Dictionary<string, object> Files { get; }
}
