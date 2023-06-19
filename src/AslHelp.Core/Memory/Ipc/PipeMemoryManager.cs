using System;
using System.Diagnostics;
using System.IO.Pipes;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;
using AslHelp.Common.Memory.Ipc.Requests;
using AslHelp.Common.Memory.Ipc.Responses;
using AslHelp.Core.Diagnostics.Logging;

namespace AslHelp.Core.Memory.Ipc;

public sealed class PipeMemoryManager : MemoryManagerBase
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
            string msg = "[Deref] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = "[Deref] Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = "[Deref] Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        fixed (int* pOffsets = offsets)
        {
            DerefRequest request = new(PipeRequest.Deref, baseAddress, offsets.Length, pOffsets);
            DerefResponse response = _pipe.Transact<DerefRequest, DerefResponse>(request);

            if (response.Code != PipeResponse.Success)
            {
                string msg = $"[Deref] Failed to dereference pointer. Response: {response}";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            return response.Result;
        }
    }

    public override unsafe bool TryDeref(out nuint result, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "[Deref] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = "[Deref] Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            result = default;
            return false;
        }

        fixed (int* pOffsets = offsets)
        {
            DerefRequest request = new(PipeRequest.Deref, baseAddress, offsets.Length, pOffsets);
            DerefResponse response = _pipe.Transact<DerefRequest, DerefResponse>(request);

            if (response.Code != PipeResponse.Success)
            {
                result = default;
                return false;
            }

            result = response.Result;
            return true;
        }
    }

    public override T Read<T>(nuint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public override bool TryRead<T>(out T result, nuint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public override void ReadSpan<T>(Span<T> buffer, nuint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public override bool TryReadSpan<T>(Span<T> buffer, nuint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public override bool Write<T>(T value, nuint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public override bool WriteSpan<T>(ReadOnlySpan<T> values, nuint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public override void Dispose()
    {
        base.Dispose();

        _pipe.Dispose();
    }
}
