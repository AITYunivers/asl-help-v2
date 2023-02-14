using AslHelp.Core.Exceptions;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.LiveSplitInterop;

public partial class Basic
{
    protected DebugLogger _dbgLogger = new();
    protected FileLogger _fileLogger;

    //public TimerControl Timer { get; } = new();
    //public SettingsCreator Settings { get; } = new();
    //public TextComponentManager Texts { get; } = new();

    public void StartFileLogger(string filePath, int maxLines = 4096, int linesToErase = 512)
    {
        _fileLogger?.Stop();

        if (Methods.CurrentMethod != "startup")
        {
            string msg = "[IO] The file logger may not be started outside of the 'startup' action.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        try
        {
            _fileLogger = new(filePath, maxLines, linesToErase);
            _fileLogger.Start();
        }
        catch (Exception ex)
        {
            Debug.Error($"""
                [IO] Was unable to create the file logger:
                {ex}
                """);
        }
    }

    public void StartBench(string id)
    {
        _dbgLogger.StartBenchmark(id);
        _fileLogger?.StartBenchmark(id);
    }

    public void StopBench(string id)
    {
        _dbgLogger.StopBenchmark(id);
        _fileLogger?.StopBenchmark(id);
    }

    public void Log(object output)
    {
        _dbgLogger.Log($"[{GameName}] {output}");
        _fileLogger?.Log($"[{GameName}] {output}");
    }
}
