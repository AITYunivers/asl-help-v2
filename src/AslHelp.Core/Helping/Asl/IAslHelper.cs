using AslHelp.Core.Collections;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.LiveSplitInterop.Texts;
using AslHelp.Core.Memory.Models;

namespace AslHelp.Core.Helping.Asl;

public interface IAslHelper : IHelper
{
    bool Is64Bit { get; }
    byte PtrSize { get; }

    Module MainModule { get; }
    ModuleCache Modules { get; }

    IEnumerable<MemoryPage> Pages { get; }

    TextComponentController Texts { get; }
    TimerController Timer { get; }
}
