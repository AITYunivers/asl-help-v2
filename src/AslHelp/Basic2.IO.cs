using AslHelp.Core.IO;
using AslHelp.Core.IO.Logging;

public partial class Basic
{
    private readonly List<FileWatcher> _fileWatchers = new();

    protected override void CreateFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512)
    {
        FileLogger logger = new(filePath, maxLines, linesToErase);
        logger.Start();

        _logger.Add(logger);
    }

    protected override FileWatcher CreateFileWatcher(string filePath)
    {
        FileWatcher watcher = new(filePath);
        _fileWatchers.Add(watcher);

        return watcher;
    }
}
