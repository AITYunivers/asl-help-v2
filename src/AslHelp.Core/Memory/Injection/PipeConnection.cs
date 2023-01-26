using System.IO.Pipes;

namespace AslHelp.Core.Memory.Pipes;

public class PipeConnection : IDisposable
{
    private readonly NamedPipeClientStream _pipe;

    public PipeConnection(string pipeName)
    {
        _pipe = new(pipeName);
    }

    public bool Open()
    {
        _pipe.Connect();

        return _pipe.IsConnected && _pipe.CanWrite && _pipe.CanRead;
    }

    public TResponse Transact<TSerializer, TRequest, TResponse>(TRequest request)
        where TSerializer : struct, IPipeSerializer<TRequest, TResponse>
    {
        Unsafe.SkipInit(out TSerializer serializer);

        serializer.Write(request, _pipe);
        return serializer.Read(_pipe);
    }

    public void Dispose()
    {
        _pipe.Dispose();
    }
}
