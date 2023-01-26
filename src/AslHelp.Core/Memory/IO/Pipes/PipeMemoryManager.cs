using AslHelp.Core.Memory.Pipes;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.IO.Pipes;

public class PipeMemoryManager : MemoryManagerBase
{
    private readonly PipeConnection _pipe;

    public PipeMemoryManager(PipeConnection pipe)
    {
        _pipe = pipe;
    }

    public sealed override bool TryDeref(out nint result, nint baseAddress, params int[] offsets)
    {

    }

    public sealed override bool TryRead<T>(out T result, nint baseAddress, params int[] offsets)
    {

    }

    public sealed override bool TryReadSpan<T>(Span<T> buffer, nint baseAddress, params int[] offsets)
    {

    }

    public sealed override bool TryReadString(out string result, int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {

    }

    public sealed override bool Write<T>(T value, nint baseAddress, params int[] offsets)
    {

    }

    public sealed override bool WriteSpan<T>(ReadOnlySpan<T> values, nint baseAddress, params int[] offsets)
    {

    }
}
