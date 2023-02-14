using System.IO.Pipes;
using AslHelp.Core.Exceptions;
using AslHelp.Core.IO.Logging;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.IO;

public sealed class PipeMemoryManager : MemoryManagerBase
{
    private readonly NamedPipeClientStream _pipe;

    public PipeMemoryManager(Process process, NamedPipeClientStream pipe)
        : this(process, null, pipe) { }

    public PipeMemoryManager(Process process, LoggerBase logger, NamedPipeClientStream pipe)
        : base(process, logger)
    {
        _pipe = pipe;
    }

    public sealed override bool TryDeref(out nint result, nint baseAddress, params int[] offsets)
    {
        EnsurePipeState();
    }

    public sealed override bool TryRead<T>(out T result, nint baseAddress, params int[] offsets)
    {
        EnsurePipeState();
    }

    public sealed override bool TryReadSpan<T>(Span<T> buffer, nint baseAddress, params int[] offsets)
    {
        EnsurePipeState();
    }

    public sealed override bool TryReadString(out string result, int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        EnsurePipeState();
    }

    public sealed override bool Write<T>(T value, nint baseAddress, params int[] offsets)
    {
        EnsurePipeState();
    }

    public sealed override bool WriteSpan<T>(ReadOnlySpan<T> values, nint baseAddress, params int[] offsets)
    {
        EnsurePipeState();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsurePipeState()
    {
        if (!_pipe.IsConnected)
        {
            ThrowHelper.Throw.InvalidOperation("Pipe is not connected.");
        }

        if (!_pipe.CanRead)
        {
            ThrowHelper.Throw.InvalidOperation("Pipe stream was not readable.");
        }

        if (!_pipe.CanWrite)
        {
            ThrowHelper.Throw.InvalidOperation("Pipe stream was not writable.");
        }
    }
}
