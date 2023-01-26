using System.Buffers;
using System.IO.Pipes;
using AslHelp.Core.Memory.Pipes;
using CommunityToolkit.HighPerformance;

namespace AslHelp.Core.Memory.IO.Pipes;

internal struct ReadSpan<T>
    : IPipeSerializer<ReadSpan<T>.Request, ReadSpan<T>.Response>
    where T : unmanaged
{
    public readonly unsafe record struct Request(int Length, nint Address);
    public readonly record struct Response(bool Success, T[] Results);

    private int _bytes;
    private int _length;

    public unsafe void Write(Request request, NamedPipeClientStream pipe)
    {
        _bytes = request.Length * sizeof(T);
        _length = request.Length;

        pipe.Write(PipeRequestCode.ReadSpan);
        pipe.Write<long>(request.Address);
        pipe.Write(_bytes);
    }

    public Response Read(NamedPipeClientStream pipe)
    {
        PipeResponseCode code = pipe.Read<PipeResponseCode>();

        if (code == PipeResponseCode.Success)
        {
            T[] rented = ArrayPool<T>.Shared.Rent(_length);

            Span<byte> bytes = stackalloc byte[_bytes];
            pipe.Read(bytes);
            MemoryMarshal.Cast<byte, T>(bytes).CopyTo(rented);

            return new(true, rented);
        }
        else
        {
            return new(false, Array.Empty<T>());
        }
    }
}
