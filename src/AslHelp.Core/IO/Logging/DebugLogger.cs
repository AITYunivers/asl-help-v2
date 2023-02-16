namespace AslHelp.Core.IO.Logging;

public sealed class DebugLogger : LoggerBase
{
    public override void Start() { }

    public override void Log()
    {
        LiveSplit.Options.Log.Info("");
    }

    public override void Log(object output)
    {
        LiveSplit.Options.Log.Info(output?.ToString());
    }

    public override void Stop() { }
}
