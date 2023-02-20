using AslHelp.Core.Collections;
using AslHelp.Core.IO;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.LiveSplitInterop.Texts;
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

    Dictionary<string, FileWatcher> Files { get; }
    TextComponentController Texts { get; }
    TimerController Timer { get; }
}
