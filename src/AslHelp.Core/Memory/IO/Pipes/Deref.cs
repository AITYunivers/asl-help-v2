using System.IO.Pipes;
using AslHelp.Core.Memory.Pipes;
using CommunityToolkit.HighPerformance;

namespace AslHelp.Core.Memory.IO.Pipes;

internal readonly struct Deref
    : IPipeSerializer<Deref.Request, Deref.Response>
{
    public readonly record struct Request(nint Address, int[] Offsets);
    public readonly record struct Response(bool Success, nint Result);

    public void Write(Request request, NamedPipeClientStream pipe)
    {
        pipe.Write(PipeRequestCode.Deref);
        pipe.Write<long>(request.Address);
        pipe.Write(request.Offsets.Length);
        pipe.Write(MemoryMarshal.AsBytes<int>(request.Offsets));
    }

    public Response Read(NamedPipeClientStream pipe)
    {
        PipeResponseCode code = pipe.Read<PipeResponseCode>();

        if (code == PipeResponseCode.Success)
        {
            return new(true, (nint)pipe.Read<long>());
        }
        else
        {
            return new(false, default);
        }
    }
}
