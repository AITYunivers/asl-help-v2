using System.Diagnostics;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.IO;
using AslHelp.Core.LiveSplitInterop.Control;
using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Memory.Pointers;
using AslHelp.Core.Memory.Pointers.Initialization;

namespace AslHelp.Core.Helpers.Asl.Contracts;

public partial interface IAslHelper
{
    ILogger Logger { get; }

    TimerController Timer { get; }
    TextComponentController Texts { get; }

    FileWatcher CreateFileWatcher(string fileName);

    string GameName { get; }
    Process? Game { get; set; }

    IMemoryManager? Memory { get; }
    IPointerFactory? Pointers { get; }

    IPointer this[string name] { get; set; }
    void MapPointerValuesToCurrent();

    bool Reject(params uint[] moduleMemorySizes);
    bool Reject(string moduleName, params uint[] moduleMemorySizes);
    bool Reject(Module module, params uint[] moduleMemorySizes);

    void OnExit();
    void OnShutdown();
}
