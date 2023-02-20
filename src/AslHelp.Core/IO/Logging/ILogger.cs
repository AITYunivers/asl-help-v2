namespace AslHelp.Core.IO.Logging;

public interface ILogger
{
    void Log();
    void Log(object output);

    void Start();
    void Stop();
}
