using AslHelp.Core.Collections;

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

    void Update();
    uint Tick { get; }

    void Log(object output);
}
