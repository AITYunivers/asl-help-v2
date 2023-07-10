using System.Diagnostics;
using System.IO.Pipes;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;
using AslHelp.Common.Memory.Ipc;
using AslHelp.Core.Diagnostics.Logging;

namespace AslHelp.Core.Memory.Ipc;

public class PipeMemoryManager : MemoryManagerBase
{
    private readonly NamedPipeClientStream _pipe;

    public PipeMemoryManager(Process process, string pipeName, int timeout = -1)
        : base(process)
    {
        _pipe = new(pipeName);
        _pipe.Connect(timeout);
    }

    public PipeMemoryManager(Process process, ILogger logger, string pipeName, int timeout = -1)
        : base(process, logger)
    {
        _pipe = new(pipeName);
        _pipe.Connect(timeout);
    }

    public PipeMemoryManager(Process process, NamedPipeClientStream pipe)
        : base(process)
    {
        if (!pipe.IsConnected)
        {
            ThrowHelper.ThrowInvalidOperationException("Pipe stream was not connected.");
        }

        if (!pipe.CanRead)
        {
            ThrowHelper.ThrowInvalidOperationException("Pipe stream was not readable.");
        }

        if (!pipe.CanWrite)
        {
            ThrowHelper.ThrowInvalidOperationException("Pipe stream was not writable.");
        }

        _pipe = pipe;
    }

    public PipeMemoryManager(Process process, ILogger logger, NamedPipeClientStream pipe)
        : base(process, logger)
    {
        if (!pipe.IsConnected)
        {
            ThrowHelper.ThrowInvalidOperationException("Pipe stream was not connected.");
        }

        if (!pipe.CanRead)
        {
            ThrowHelper.ThrowInvalidOperationException("Pipe stream was not readable.");
        }

        if (!pipe.CanWrite)
        {
            ThrowHelper.ThrowInvalidOperationException("Pipe stream was not writable.");
        }

        _pipe = pipe;
    }

    public override unsafe nuint Deref(nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = "Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = "Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        fixed (int* pOffsets = offsets)
        {
            nuint result;

            _pipe.Write(PipeRequest.Deref);
            _pipe.Write<DerefRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)&result));

            PipeResponse response = _pipe.Read<PipeResponse>();
            if (response != PipeResponse.Success)
            {
                string msg = $"Failed to dereference pointer ({response} {(int)response}).";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            return result;
        }
    }

    public override unsafe bool TryDeref(out nuint result, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = "Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            result = default;
            return false;
        }

        fixed (int* pOffsets = offsets)
        fixed (nuint* pResult = &result)
        {
            _pipe.Write(PipeRequest.Deref);
            _pipe.Write<DerefRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)pResult));

            return _pipe.TryRead(out PipeResponse response) && response == PipeResponse.Success;
        }
    }

    protected override unsafe void Read<T>(T* buffer, uint length, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = "Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = "Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        fixed (int* pOffsets = offsets)
        {
            _pipe.Write(PipeRequest.Read);
            _pipe.Write<ReadRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)buffer,
                length));

            PipeResponse response = _pipe.Read<PipeResponse>();
            if (response != PipeResponse.Success)
            {
                string msg = $"Failed to read value ({response} {(int)response}).";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }
        }
    }

    protected override unsafe bool TryRead<T>(T* buffer, uint length, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = "Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            return false;
        }

        fixed (int* pOffsets = offsets)
        {
            _pipe.Write(PipeRequest.Read);
            _pipe.Write<ReadRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)buffer,
                length));

            return _pipe.TryRead(out PipeResponse response) && response == PipeResponse.Success;
        }
    }

    protected override unsafe void Write<T>(T* data, uint length, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = "Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = "Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        fixed (int* pOffsets = offsets)
        {
            _pipe.Write(PipeRequest.Read);
            _pipe.Write<WriteRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)data,
                length));

            PipeResponse response = _pipe.Read<PipeResponse>();
            if (response != PipeResponse.Success)
            {
                string msg = $"Failed to write value ({response} {(int)response}).";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }
        }
    }

    protected override unsafe bool TryWrite<T>(T* data, uint length, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = "Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = "Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        fixed (int* pOffsets = offsets)
        {
            _pipe.Write(PipeRequest.Read);
            _pipe.Write<WriteRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)data,
                length));

            return _pipe.TryRead(out PipeResponse response) && response == PipeResponse.Success;
        }
    }

    public override void Dispose()
    {
        base.Dispose();

        _pipe.Write(PipeRequest.Close);
        _pipe.Dispose();
    }
}
