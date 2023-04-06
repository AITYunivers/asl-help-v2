using AslHelp.Core.Collections;
using AslHelp.Core.IO;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.LiveSplitInterop.Settings;
using AslHelp.Core.LiveSplitInterop.Texts;
using AslHelp.Core.Memory.Models;
using AslHelp.Core.Memory.Pointers;

namespace AslHelp.Core.Helping;

public interface IAslHelper
    : IHelper
{
    string GameName { get; set; }

    bool Is64Bit { get; }
    byte PtrSize { get; }

    Module MainModule { get; }
    ModuleCache Modules { get; }

    IEnumerable<MemoryPage> Pages(bool allPages);

    TextComponentController Texts { get; }
    TimerController Timer { get; }
    SettingsCreator Settings { get; }

    IPointer this[string name] { get; set; }

    void GenerateCode();

    void CreateFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512);
    FileWatcher CreateFileWatcher(string filePath);
}
