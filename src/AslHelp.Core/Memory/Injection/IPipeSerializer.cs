using System.IO.Pipes;

namespace AslHelp.Core.Memory.Pipes;

public interface IPipeSerializer<TRequest, TResponse>
{
    void Write(TRequest request, NamedPipeClientStream pipe);
    TResponse Read(NamedPipeClientStream pipe);
}
