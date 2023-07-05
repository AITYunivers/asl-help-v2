using System.Diagnostics;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Memory.Pointers;
using AslHelp.Core.Memory.Pointers.Initialization;

namespace AslHelp.Core.Helpers.Asl.Contracts;

public partial interface IAslHelper
{
    ILogger Logger { get; }

    string GameName { get; set; }
    Process? Game { get; set; }

    IMemoryManager? Memory { get; }
    IPointerFactory? Pointers { get; }

    IPointer this[string name] { get; set; }
    void MapPointerValuesToCurrent();

    void OnExit();
    void OnShutdown();
}
