using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using AslHelp.Common.Exceptions;
using AslHelp.Common.Pipes;
using AslHelp.Core.IO.Logging;
using CommunityToolkit.HighPerformance;

namespace AslHelp.Core.Memory.IO;

public sealed unsafe class PipeMemoryManager : MemoryManagerBase
{
    private readonly NamedPipeClientStream _pipe;

    public PipeMemoryManager(Process process, string pipeName)
        : this(process, null, pipeName) { }

    public PipeMemoryManager(Process process, ILogger logger, string pipeName, int timeout = -1)
        : base(process, logger)
    {
        _pipe = new(pipeName);
        _pipe.Connect(timeout);
    }

    public PipeMemoryManager(Process process, NamedPipeClientStream pipe)
        : this(process, null, pipe) { }

    public PipeMemoryManager(Process process, ILogger logger, NamedPipeClientStream pipe)
        : base(process, logger)
    {
        if (!pipe.IsConnected)
        {
            ThrowHelper.ThrowInvalidOperationException("Pipe was not connected.");
        }

        if (!pipe.CanRead)
        {
            ThrowHelper.ThrowInvalidOperationException("Pipe was not readable.");
        }

        if (!pipe.CanWrite)
        {
            ThrowHelper.ThrowInvalidOperationException("Pipe was not writable.");
        }

        _pipe = pipe;
    }

    public bool IsConnected => _pipe.IsConnected && _pipe.CanRead && _pipe.CanWrite;

    public sealed override bool TryDeref(out nint result, nint baseAddress, params int[] offsets)
    {
        if (!IsConnected || baseAddress == 0)
        {
            result = default;
            return false;
        }

        _pipe.Write(PipeRequestCode.Deref);

        _pipe.Write<long>(baseAddress);
        _pipe.Write(offsets.Length);
        _pipe.Write(MemoryMarshal.AsBytes<int>(offsets));

        PipeResponseCode code = _pipe.Read<PipeResponseCode>();
        if (code != PipeResponseCode.Success)
        {
            Log($"Deref failed with code {(int)code}.");

            result = default;
            return false;
        }

        result = (nint)_pipe.Read<long>();
        return result != default;
    }

    public sealed override unsafe bool TryRead<T>(out T result, nint baseAddress, params int[] offsets)
    {
        if (!IsConnected)
        {
            result = default;
            return false;
        }

        _pipe.Write(PipeRequestCode.Read);

        _pipe.Write<long>(baseAddress);
        _pipe.Write(offsets.Length);
        _pipe.Write(MemoryMarshal.AsBytes<int>(offsets));
        _pipe.Write(Native.GetTypeSize<T>(PtrSize));

        PipeResponseCode code = _pipe.Read<PipeResponseCode>();
        if (code != PipeResponseCode.Success)
        {
            Log($"Read<{typeof(T).Name}> failed with code {(int)code}.");

            result = default;
            return false;
        }

        result = _pipe.Read<T>();
        return !Native.IsNativeIntAndZero(result);
    }

    public sealed override bool TryReadSpan<T>(Span<T> buffer, nint baseAddress, params int[] offsets)
    {
        if (!IsConnected)
        {
            return false;
        }

        _pipe.Write(PipeRequestCode.ReadSpan);

        _pipe.Write<long>(baseAddress);
        _pipe.Write(offsets.Length);
        _pipe.Write(MemoryMarshal.AsBytes<int>(offsets));
        _pipe.Write(Native.GetTypeSize<T>(PtrSize) * buffer.Length);

        PipeResponseCode code = _pipe.Read<PipeResponseCode>();
        if (code != PipeResponseCode.Success)
        {
            Log($"ReadSpan<{typeof(T).Name}> failed with code {(int)code}.");

            return false;
        }

        _ = _pipe.Read(MemoryMarshal.AsBytes(buffer));
        return true;
    }

    public sealed override unsafe bool Write<T>(T value, nint baseAddress, params int[] offsets)
    {
        if (!IsConnected)
        {
            return false;
        }

        _pipe.Write(PipeRequestCode.Write);

        _pipe.Write<long>(baseAddress);
        _pipe.Write(offsets.Length);
        _pipe.Write(MemoryMarshal.AsBytes<int>(offsets));

        _pipe.Write(Native.GetTypeSize<T>(PtrSize));
        _pipe.Write(value);

        PipeResponseCode code = _pipe.Read<PipeResponseCode>();
        if (code != PipeResponseCode.Success)
        {
            Log($"Write<{typeof(T).Name}> failed with code {(int)code}.");

            return false;
        }

        return true;
    }

    public sealed override unsafe bool WriteSpan<T>(ReadOnlySpan<T> values, nint baseAddress, params int[] offsets)
    {
        if (!IsConnected)
        {
            return false;
        }

        _pipe.Write(PipeRequestCode.Write);

        _pipe.Write<long>(baseAddress);
        _pipe.Write(offsets.Length);
        _pipe.Write(MemoryMarshal.AsBytes<int>(offsets));

        _pipe.Write(Native.GetTypeSize<T>(PtrSize) * values.Length);
        _pipe.Write(MemoryMarshal.AsBytes(values));

        PipeResponseCode code = _pipe.Read<PipeResponseCode>();

        return code == PipeResponseCode.Success;
    }

    public override void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        base.Dispose();

        _pipe.Write(PipeRequestCode.ClosePipe);
        _pipe.Dispose();
    }
}
