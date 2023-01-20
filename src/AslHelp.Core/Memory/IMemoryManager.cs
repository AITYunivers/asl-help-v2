using AslHelp.Core.Collections;
using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory;

public interface IMemoryManager
    : IDisposable,
    IMemoryReader,
    IMemoryWriter,
    IMemoryScanner
{
    Process Process { get; }
    bool Is64Bit { get; }

    Module MainModule { get; }
    ModuleCache Modules { get; }

    void Update();
    uint Tick { get; }

    void Log(object output);
}
