using System.IO.Pipes;
using AslHelp.Core.Memory.Pipes;
using CommunityToolkit.HighPerformance;

namespace AslHelp.Core.Memory.IO.Pipes;

internal readonly struct Read<T>
    : IPipeSerializer<Read<T>.Request, Read<T>.Response>
    where T : unmanaged
{
    public readonly record struct Request(nint Address);
    public readonly record struct Response(bool Success, T Result);

    public unsafe void Write(Request request, NamedPipeClientStream pipe)
    {
        pipe.Write(PipeRequestCode.Read);
        pipe.Write<long>(request.Address);
        pipe.Write(sizeof(T));
    }

    Response IPipeSerializer<Request, Response>.Read(NamedPipeClientStream pipe)
    {
        PipeResponseCode code = pipe.Read<PipeResponseCode>();

        if (code == PipeResponseCode.Success)
        {
            return new(true, pipe.Read<T>());
        }
        else
        {
            return new(false, default);
        }
    }
}
