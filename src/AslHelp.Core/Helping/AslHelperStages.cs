using AslHelp.Core.Memory.Pointers;

namespace AslHelp.Core.Helping;

public interface IAslInitStage
{
    IAslAfterInitStage InitForAsl();
}

public interface IAslGenerateStage
{
    IAslAfterGenerateStage GenerateCode();
}

public interface IAslIOStage
{
    IAslAfterGenerateStage CreateFileLogger(string filePath, int maxLines = 4096, int linesToErates = 512);
    IAslAfterGenerateStage CreateFileWatcher(string filePath);
    IAslAfterGenerateStage CreateFileWatcher(string filePath, string name);
}

public interface IAslPointersStage
{
    PointerFactory Pointers { get; }

    IAslPointersStage
}

public interface IAslAfterInitStage
    : IAslGenerateStage,
    IAslIOStage,
    IAslPointersStage
{ }

public interface IAslAfterGenerateStage
    : IAslIOStage,
    IAslPointersStage
{ }
