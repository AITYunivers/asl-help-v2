using AslHelp.Core.Exceptions;

namespace AslHelp.Core.IO.Logging;

public sealed class FileLogger
    : ILogger,
    IDisposable
{
    private readonly Queue<string> _queuedLines = new();
    private CancellationTokenSource _cancelSource = new();
    private readonly ManualResetEvent _resetEvent = new(false);

    private int _lineNumber;
    private readonly int _maximumLines;
    private readonly int _linesToErase;

    private bool _isRunning;

    public FileLogger(string path, int maximumLines, int linesToErase)
    {
        if (maximumLines < 1)
        {
            string msg = "The file logger must allow at least 1 line.";
            ThrowHelper.Throw.ArgumentOutOfRange(nameof(maximumLines), msg);
        }

        if (linesToErase < 1)
        {
            string msg = "File logger must erase at least 1 line.";
            ThrowHelper.Throw.ArgumentOutOfRange(nameof(linesToErase), msg);
        }

        if (linesToErase > maximumLines)
        {
            string msg = "The file logger's maximum lines must be greater or equal to the amount of lines to erase.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        FilePath = Path.GetFullPath(path);
        _maximumLines = maximumLines;
        _linesToErase = linesToErase;
    }

    public string FilePath { get; }

    public void Start()
    {
        if (_isRunning)
        {
            return;
        }

        _ = Task.Run(() =>
        {
            _isRunning = true;
            _cancelSource = new();
            _lineNumber = 0;

            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Dispose();
            }
            else
            {
                using StreamReader reader = new(FilePath);

                while (!_cancelSource.IsCancellationRequested && reader.ReadLine() is not null)
                {
                    _lineNumber++;
                }
            }

            string output;

            while (true)
            {
                _ = _resetEvent.WaitOne();
                if (_cancelSource.IsCancellationRequested)
                {
                    break;
                }

                lock (_queuedLines)
                {
                    if (!_queuedLines.Any())
                    {
                        _ = _resetEvent.Reset();
                        continue;
                    }

                    output = _queuedLines.Dequeue();
                }

                WriteLine(output);
            }
        }, _cancelSource.Token);
    }

    public void Log()
    {
        if (!_isRunning)
        {
            string msg = "Logger is not running.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        lock (_queuedLines)
        {
            _queuedLines.Enqueue("");
            _ = _resetEvent.Set();
        }
    }

    public void Log(object output)
    {
        if (!_isRunning)
        {
            string msg = "Logger is not running.";
            ThrowHelper.Throw.InvalidOperation(msg);
        }

        lock (_queuedLines)
        {
            _queuedLines.Enqueue($"[{DateTime.Now:dd-MMM-yy HH:mm:ss.fff}] :: {output}");
            _ = _resetEvent.Set();
        }
    }

    public void Stop()
    {
        if (!_isRunning)
        {
            return;
        }

        _cancelSource.Cancel();
        _ = _resetEvent.Set();
        _queuedLines.Clear();
    }

    private void WriteLine(string output)
    {
        if (_lineNumber >= _maximumLines)
        {
            FlushLines();
        }

        try
        {
            using StreamWriter writer = new(FilePath, true);

            writer.WriteLine(output);
            _lineNumber++;
        }
        catch (Exception ex)
        {
            Debug.Error($"""
                FileLogger was unable to write to the output:
                {ex}
                """);
        }
    }

    private void FlushLines()
    {
        string tempFile = $"{FilePath}-temp";
        string[] lines = File.ReadAllLines(FilePath).Skip(_linesToErase).ToArray();

        File.WriteAllLines(tempFile, lines);

        try
        {
            File.Copy(tempFile, FilePath, true);
            _lineNumber = lines.Length;
        }
        catch (Exception ex)
        {
            Debug.Error($"""
                FileLogger failed replacing log file with temporary log file:
                {ex}
                """);
        }
        finally
        {
            try
            {
                File.Delete(tempFile);
            }
            catch (Exception ex)
            {
                Debug.Error($"""
                    FileLogger failed deleting temporary log file:
                    {ex}
                    """);
            }
        }
    }

    public void Dispose()
    {
        Stop();
    }
}
