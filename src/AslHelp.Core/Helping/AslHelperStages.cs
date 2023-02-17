using AslHelp.Core.LiveSplitInterop.Settings;

namespace AslHelp.Core.Helping;

public interface IAslInitStage
{
    IAslGenerateStage InitForAsl();
}

public interface IAslGenerateStage
{
    IAslIoStage GenerateCode();
}

public interface IAslSettingsPhase
{
    SettingsCreator Settings { get; }
}

public interface IAslIoStage
{
    IAslIoStage CreateFileLogger(string filePath, int maxLines = 4096, int linesToErates = 512);
}
