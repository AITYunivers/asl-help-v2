namespace AslHelp.Core.IO.Logging;

public sealed class DebugLogger : ILogger
{
    public void Start() { }

    public void Log()
    {
        LiveSplit.Options.Log.Info("");
    }

    public void Log(object output)
    {
        LiveSplit.Options.Log.Info(output?.ToString());
    }

    public void Stop() { }
}
