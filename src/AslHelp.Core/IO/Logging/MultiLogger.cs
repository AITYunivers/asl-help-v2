namespace AslHelp.Core.IO.Logging;

public sealed class MultiLogger
    : ILogger,
    IList<ILogger>
{
    private readonly List<ILogger> _loggers;

    public MultiLogger(params ILogger[] loggers)
    {
        _loggers = new(loggers);
    }

    public int Count => _loggers.Count;
    public bool IsReadOnly => ((ICollection<ILogger>)_loggers).IsReadOnly;

    public ILogger this[int index]
    {
        get => _loggers[index];
        set => _loggers[index] = value;
    }

    public void Start()
    {
        for (int i = 0; i < _loggers.Count; i++)
        {
            _loggers[i]?.Start();
        }
    }

    public void Log()
    {
        Log("");
    }

    public void Log(object output)
    {
        for (int i = 0; i < _loggers.Count; i++)
        {
            _loggers[i]?.Log(output);
        }
    }

    public void Stop()
    {
        for (int i = 0; i < _loggers.Count; i++)
        {
            _loggers[i]?.Stop();
        }
    }

    public void Add(ILogger item)
    {
        _loggers.Add(item);
    }

    public void Insert(int index, ILogger item)
    {
        _loggers.Insert(index, item);
    }

    public bool Remove(ILogger item)
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

    public bool Contains(ILogger item)
    {
        return _loggers.Contains(item);
    }

    public int IndexOf(ILogger item)
    {
        return _loggers.IndexOf(item);
    }

    public void CopyTo(ILogger[] array, int arrayIndex)
    {
        _loggers.CopyTo(array, arrayIndex);
    }

    public IEnumerator<ILogger> GetEnumerator()
    {
        return _loggers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _loggers.GetEnumerator();
    }
}
