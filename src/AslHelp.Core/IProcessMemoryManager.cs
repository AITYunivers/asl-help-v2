using AslHelp.Core.Collections;

namespace AslHelp.Core;

public interface IProcessMemoryManager : IDisposable
{
    Process Process { get; }

    Module MainModule { get; }
    ModuleCache Modules { get; }

    uint Tick { get; }

    void Log(object output);
}
