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
    byte PointerSize { get; }

    Module MainModule { get; }
    ModuleCache Modules { get; }

    IEnumerable<MemoryPage> Pages(bool allPages);

    uint Tick { get; }
    void Update();

    nuint ReadRelative(nuint address);
    bool TryReadRelative(nuint address, out nuint result);

    void Log(object output);
}
