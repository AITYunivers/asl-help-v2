using AslHelp.Core.Collections;
using AslHelp.Core.Memory.Models;

namespace AslHelp.Core.Memory.IO;

public interface IMemoryManager
    : IDisposable,
    IMemoryReader,
    IMemoryWriter,
    IMemoryScanner
{
    Process Process { get; }
    bool Is64Bit { get; }
    byte PtrSize { get; }

    Module MainModule { get; }
    ModuleCache Modules { get; }
    IEnumerable<MemoryPage> Pages(bool allPages);

    void Update();
    uint Tick { get; }

    void Log(object output);
}
