using AslHelp.Core.LiveSplitInterop.Settings;

namespace AslHelp.Core.Helping;

public interface IAslInitStage
{
    IAslGenerateStage InitForAsl();
}

public interface IAslGenerateStage
{
    IAslHelper GenerateCode();
}

public interface IAslSettingsPhase
{
    SettingsCreator Settings { get; }
}

public interface IAslIOHelper
{
    IAslIOHelper CreateFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512);
    IAslIOHelper CreateFileWatcher(string filePath);
    IAslIOHelper CreateFileWatcher(string filePath, string name);
}
