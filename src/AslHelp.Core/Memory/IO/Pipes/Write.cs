using System.IO.Pipes;
using AslHelp.Core.Memory.Pipes;
using CommunityToolkit.HighPerformance;

namespace AslHelp.Core.Memory.IO.Pipes;

internal readonly struct Write<T>
    : IPipeSerializer<Write<T>.Request, Write<T>.Response>
    where T : unmanaged
{
    public readonly record struct Request(T Value, nint Address);
    public readonly record struct Response(bool Success);

    void IPipeSerializer<Request, Response>.Write(Request request, NamedPipeClientStream pipe)
    {
        pipe.Write(PipeRequestCode.Write);
    }

    public Response Read(NamedPipeClientStream pipe)
    {
        PipeResponseCode code = pipe.Read<PipeResponseCode>();

        if (code == PipeResponseCode.Success)
        {
            return new(true);
        }
        else
        {
            return new(false);
        }
    }
}
