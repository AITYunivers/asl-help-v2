using AslHelp.Core.IO;
using AslHelp.Core.LiveSplitInterop;
using AslHelp.Core.LiveSplitInterop.Settings;
using AslHelp.Core.LiveSplitInterop.Texts;
using AslHelp.Core.Memory.Pointers;

namespace AslHelp.Core.Helping;

public interface IAslHelperInitStage
{
    IAslHelper Init(bool generateCode);
}

public interface IAslHelper
    : IHelper
{
    string GameName { get; set; }

    TextComponentController Texts { get; }
    TimerController Timer { get; }
    SettingsCreator Settings { get; }

    IPointer this[string name] { get; set; }

    void CreateFileLogger(string filePath, int maxLines, int linesToErase);
    FileWatcher CreateFileWatcher(string filePath);
}
