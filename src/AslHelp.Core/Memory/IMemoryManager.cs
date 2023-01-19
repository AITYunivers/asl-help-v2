using AslHelp.Core.Collections;
using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory;

public interface IMemoryManager
    : IMemoryReader,
    IMemoryWriter,
    IMemoryScanner
{
    Process Process { get; }
    bool Is64Bit { get; }

    Module MainModule { get; }
    ModuleCache Modules { get; }

    uint Tick { get; }

    void Log(object output);
}
