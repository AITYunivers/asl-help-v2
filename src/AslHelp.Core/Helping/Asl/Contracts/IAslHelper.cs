using AslHelp.Core.IO;
using AslHelp.Core.LiveSplitInterop.Settings;
using AslHelp.Core.LiveSplitInterop.Texts;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.Memory.Pointers;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Helping.Asl.Contracts;

public partial interface IAslHelper
{
    string GameName { get; }
    ILogger Logger { get; }

    TextComponentController Texts { get; }
    TimerController Timer { get; }
    SettingsCreator Settings { get; }

    Process Game { get; set; }
    IMemoryManager Memory { get; }

    PointerFactory Pointers { get; }
    IPointer this[string name] { get; set; }
    IAslHelper MapPointerValuesToCurrent();

    IAslHelper CreateFileLogger(string filePath);
    IAslHelper CreateFileLogger(string filePath, int maxLines);
    IAslHelper CreateFileLogger(string filePath, int maxLines, int linesToErase);
    FileWatcher CreateFileWatcher(string filePath);

    IAslHelper Exit();
    IAslHelper Shutdown();
}
