using System.IO.Pipes;
using AslHelp.Core.Memory.Pipes;
using CommunityToolkit.HighPerformance;

namespace AslHelp.Core.Memory.IO.Pipes;

public class PipeConnection : IDisposable
{
    private readonly NamedPipeClientStream _pipe;

    public PipeConnection(NamedPipeClientStream pipe)
    {
        _pipe = pipe;
    }

    public bool TryTransact<TTransaction, TRequest, TResponse>(TRequest request, out TResponse response)
        where TTransaction : IPipeTransaction<TRequest, TResponse>
        where TRequest : struct, IPipeRequest
        where TResponse : struct, IPipeResponse
    {
        Unsafe.SkipInit(out TTransaction transact);

        if (transact.Write(_pipe, request) == PipeResponseCode.Success)
        {
            response = transact.Read(_pipe);
            return true;
        }
        else
        {
            response = default;
            return false;
        }
    }

    public void Dispose()
    {
        _pipe.Dispose();
    }
}

public interface IPipeTransaction<in TRequest, out TResponse>
    where TRequest : struct, IPipeRequest
    where TResponse : struct, IPipeResponse
{
    PipeResponseCode Write(NamedPipeClientStream pipe, TRequest data);
    TResponse Read(NamedPipeClientStream pipe);
}

public interface IPipeRequest { }
public interface IPipeResponse { }

internal struct Deref : IPipeTransaction<Deref.Request, Deref.Response>
{
    public PipeResponseCode Write(NamedPipeClientStream pipe, Request data)
    {
        pipe.Write(PipeRequestCode.Deref);

        pipe.Write<long>(data.BaseAddress);
        pipe.Write(data.Offsets.Length);
        pipe.Write(MemoryMarshal.AsBytes<int>(data.Offsets));

        return pipe.Read<PipeResponseCode>();
    }

    public Response Read(NamedPipeClientStream pipe)
    {
        return pipe.Read<Response>();
    }

    public record struct Request(nint BaseAddress, int[] Offsets) : IPipeRequest;
    public record struct Response(nint Result) : IPipeResponse;
}
