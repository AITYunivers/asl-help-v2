using AslHelp.Core.Memory.Pointers;

namespace AslHelp.Core.Helping;

public interface IAslInitStage
{
    IAslGenerateStage InitForAsl();
}

public interface IAslGenerateStage
{
    IAslHelper GenerateCode();
}

public interface IAslIOStage
{
    IAslIOStage CreateFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512);
    IAslIOStage CreateFileWatcher(string filePath);
    IAslIOStage CreateFileWatcher(string filePath, string name);
}
