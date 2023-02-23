namespace AslHelp.Core.Helping;

public interface IAslIOStage
{
    IAslIOStage CreateFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512);
    IAslIOStage CreateFileWatcher(string filePath);
    IAslIOStage CreateFileWatcher(string filePath, string name);
}

public interface IAslSettingsStage
{

}
