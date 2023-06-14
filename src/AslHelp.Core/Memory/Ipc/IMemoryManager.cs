using System;
using System.Collections.Generic;
using System.Diagnostics;

using AslHelp.Core.Collections;

namespace AslHelp.Core.Memory.Ipc;

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
