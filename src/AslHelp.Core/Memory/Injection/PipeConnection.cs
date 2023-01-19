using System.IO.Pipes;
using System.Text;

namespace AslHelp.Core.Memory.Injection;

public class PipeConnection : IDisposable
{
    private readonly NamedPipeClientStream _pipe;
    private readonly BinaryReader _br;
    private readonly BinaryWriter _bw;

    public PipeConnection(string pipeName)
        : this(pipeName, Encoding.Default, false) { }

    public PipeConnection(string pipeName, Encoding encoding)
        : this(pipeName, encoding, false) { }

    public PipeConnection(string pipeName, Encoding encoding, bool leaveOpen)
    {
        _pipe = new(pipeName);
        _br = new(_pipe, encoding, leaveOpen);
        _bw = new(_pipe, encoding, leaveOpen);
    }

    public bool Open()
    {
        _pipe.Connect();

        return _pipe.IsConnected && _pipe.CanWrite;
    }

    public TResponse Transact<TSerializer, TRequest, TResponse>(TRequest request)
        where TSerializer : struct, IPipeSerializer<TRequest, TResponse>
    {
        Unsafe.SkipInit(out TSerializer serializer);

        serializer.Write(request, _bw);
        return serializer.Read(_br);
    }

    public void Dispose()
    {
        _pipe.Dispose();
        _br.Dispose();
        _bw.Dispose();
    }
}
