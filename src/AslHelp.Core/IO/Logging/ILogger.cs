namespace AslHelp.Core.IO.Logging;

public interface ILogger
{
    void Start();

    void Log();
    void Log(object output);

    void Stop();
}
