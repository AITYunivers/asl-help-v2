using AslHelp.Core.Exceptions;

namespace AslHelp.Core.IO.Logging;

public sealed class MultiLogger : LoggerBase, IList<LoggerBase>
{
    private readonly List<LoggerBase> _loggers;

    public MultiLogger(params LoggerBase[] loggers)
    {
        ThrowHelper.ThrowIfNull(loggers);

        _loggers = new(loggers.Length);

        for (int i = 0; i < loggers.Length; i++)
        {
            if (loggers[i] is LoggerBase logger)
            {
                _loggers.Add(logger);
            }
        }
    }

    public int Count => _loggers.Count;
    public bool IsReadOnly => false;

    public LoggerBase this[int index]
    {
        get => _loggers[index];
        set => _loggers[index] = value;
    }

    public override void Start()
    {
        for (int i = 0; i < _loggers.Count; i++)
        {
            _loggers[i].Start();
        }
    }

    public override void Log()
    {
        Log("");
    }

    public override void Log(object output)
    {
        for (int i = 0; i < _loggers.Count; i++)
        {
            _loggers[i].Log(output);
        }
    }

    public override void Stop()
    {
        for (int i = 0; i < _loggers.Count; i++)
        {
            _loggers[i].Stop();
        }
    }

    public void Add(LoggerBase item)
    {
        _loggers.Add(item);
    }

    public void Insert(int index, LoggerBase item)
    {
        _loggers.Insert(index, item);
    }

    public bool Remove(LoggerBase item)
    {
        return _loggers.Remove(item);
    }

    public void RemoveAt(int index)
    {
        _loggers.RemoveAt(index);
    }

    public void Clear()
    {
        _loggers.Clear();
    }

    public bool Contains(LoggerBase item)
    {
        return _loggers.Contains(item);
    }

    public int IndexOf(LoggerBase item)
    {
        return _loggers.IndexOf(item);
    }

    public void CopyTo(LoggerBase[] array, int arrayIndex)
    {
        _loggers.CopyTo(array, arrayIndex);
    }

    public IEnumerator<LoggerBase> GetEnumerator()
    {
        return _loggers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _loggers.GetEnumerator();
    }
}
