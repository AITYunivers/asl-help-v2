namespace AslHelp.Core.IO.Logging;

public abstract class LoggerBase
{
    private readonly Dictionary<string, Stopwatch> _stopwatches = new();
    private readonly Dictionary<string, List<double>> _averages = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public abstract void Log();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public abstract void Log(object output);

    public abstract void Start();
    public abstract void Stop();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Benchmark(string id, Action action)
    {
        if (id is null)
        {
            string msg = "[Benchmark] ID was null.";
            throw new ArgumentNullException(nameof(id), msg);
        }

        StartBenchmark(id);
        action?.Invoke();
        StopBenchmark(id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AvgBenchmark(string id, Action action)
    {
        if (id is null)
        {
            string msg = "[AvgBenchmark] ID was null.";
            throw new ArgumentNullException(nameof(id), msg);
        }

        StartAvgBenchmark(id);
        action?.Invoke();
        StopAvgBenchmark(id);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void StartBenchmark(string id)
    {
        if (id is null)
        {
            string msg = "[StartBenchmark] ID was null.";
            throw new ArgumentNullException(nameof(id), msg);
        }

        _stopwatches[id] = Stopwatch.StartNew();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void StopBenchmark(string id)
    {
        if (id is null)
        {
            string msg = "[StopBenchmark] ID was null.";
            throw new ArgumentNullException(nameof(id), msg);
        }

        _stopwatches[id].Stop();
        Log($"[BENCH] [{id}] :: {_stopwatches[id].Elapsed}");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void StartAvgBenchmark(string id)
    {
        if (id is null)
        {
            string msg = "[StartAvgBenchmark] ID was null.";
            throw new ArgumentNullException(nameof(id), msg);
        }

        _stopwatches[id] = Stopwatch.StartNew();

        if (!_averages.ContainsKey(id))
        {
            _averages[id] = new();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void StopAvgBenchmark(string id)
    {
        if (id is null)
        {
            string msg = "[StopAvgBenchmark] ID was null.";
            throw new ArgumentNullException(nameof(id), msg);
        }

        _stopwatches[id].Stop();

        _averages[id].Add(_stopwatches[id].Elapsed.TotalSeconds);
        Log($"[BENCH] [{id}] :: {_stopwatches[id].Elapsed} | Average: {_averages[id].Sum() / _averages[id].Count:0.0000000}");
    }
}
