using System.IO.Pipes;

namespace AslHelp.Common.Extensions;

public static class PipeStreamExtensions
{
    public static TResponse Transact<TRequest, TResponse>(this NamedPipeClientStream pipe, TRequest request)
        where TRequest : unmanaged
        where TResponse : unmanaged
    {
        pipe.Write(request);
        return pipe.Read<TResponse>();
    }
}
