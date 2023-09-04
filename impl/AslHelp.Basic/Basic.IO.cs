using System.Collections.Generic;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Helpers.Asl.Contracts;
using AslHelp.Core.IO;

public partial class Basic
{
    private readonly List<FileWatcher> _fileWatchers = new();

    private readonly MultiLogger _logger = new();
    public sealed override ILogger Logger => _logger;

    protected sealed override IAslHelper.Initialization LogToFileImpl(string fileName, int maxLines, int linesToErase)
    {
        FileLogger logger = new(fileName, maxLines, linesToErase);
        logger.Start();

        _logger.Add(logger);

        return this;
    }

    protected sealed override FileWatcher CreateFileWatcherImpl(string fileName)
    {
        FileWatcher watcher = new(fileName);
        _fileWatchers.Add(watcher);

        return watcher;
    }
}
