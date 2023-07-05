using AslHelp.Core.IO;
using AslHelp.Core.LiveSplitInterop.Control;
using AslHelp.Core.LiveSplitInterop.Settings;

namespace AslHelp.Core.Helpers.Asl.Contracts;

public partial interface IAslHelper
{
    public interface IInitialization
    {
        TimerController Timer { get; }
        TextComponentController Texts { get; }
        SettingsCreator Settings { get; }

        IAslHelper CreateFileLogger(string fileName, int maxLines = 4096, int linesToErase = 512);
        FileWatcher CreateFileWatcher(string fileName);

        IAslHelper SetGameName(string gameName);
        IAslHelper DoCodeGeneration(bool generateCode = true);
        IAslHelper DoInjection(int pipeConnectionTimeout = 3000);
        IAslHelper Init();
    }
}
