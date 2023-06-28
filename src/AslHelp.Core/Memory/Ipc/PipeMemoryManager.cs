using System;
using System.Diagnostics;
using System.IO.Pipes;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;
using AslHelp.Common.Memory.Ipc;
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
                string msg = $"[Deref] Failed to dereference pointer ({response} {(int)response}).";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            return result;
        }
    }

    public override unsafe bool TryDeref(out nuint result, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "[TryDeref] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = "[TryDeref] Pipe was not connected.";
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

    public override unsafe T Read<T>(nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[Read<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = $"[Read<{typeof(T).Name}>] Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = $"[Read<{typeof(T).Name}>] Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        fixed (int* pOffsets = offsets)
        {
            T result;

            _pipe.Write(PipeRequest.Read);
            _pipe.Write<ReadRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)&result,
                GetNativeSizeOf<T>()));

            PipeResponse response = _pipe.Read<PipeResponse>();
            if (response != PipeResponse.Success)
            {
                string msg = $"[Read<{typeof(T).Name}>] Failed to read value ({response} {(int)response}).";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }

            return result;
        }
    }

    public override unsafe bool TryRead<T>(out T result, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[TryRead<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = $"[TryRead<{typeof(T).Name}>] Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            result = default;
            return false;
        }

        fixed (int* pOffsets = offsets)
        fixed (T* pResult = &result)
        {
            _pipe.Write(PipeRequest.Read);
            _pipe.Write<ReadRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)pResult,
                GetNativeSizeOf<T>()));

            return _pipe.TryRead(out PipeResponse response) && response == PipeResponse.Success;
        }
    }

    public override unsafe void ReadSpan<T>(Span<T> buffer, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[ReadSpan<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = $"[ReadSpan<{typeof(T).Name}>] Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = $"[ReadSpan<{typeof(T).Name}>] Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        fixed (int* pOffsets = offsets)
        fixed (T* pBuffer = buffer)
        {
            _pipe.Write(PipeRequest.Read);
            _pipe.Write<ReadRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)pBuffer,
                GetNativeSizeOf<T>() * (uint)buffer.Length));

            PipeResponse response = _pipe.Read<PipeResponse>();
            if (response != PipeResponse.Success)
            {
                string msg = $"[ReadSpan<{typeof(T).Name}>] Failed to read values ({response} {(int)response}).";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }
        }
    }

    public override unsafe bool TryReadSpan<T>(Span<T> buffer, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[TryRead<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = $"[TryRead<{typeof(T).Name}>] Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            return false;
        }

        fixed (int* pOffsets = offsets)
        fixed (T* pBuffer = buffer)
        {
            _pipe.Write(PipeRequest.Read);
            _pipe.Write<ReadRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)pBuffer,
                GetNativeSizeOf<T>() * (uint)buffer.Length));

            return _pipe.TryRead(out PipeResponse response) && response == PipeResponse.Success;
        }
    }

    public override unsafe void Write<T>(T value, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[Write<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = $"[Write<{typeof(T).Name}>] Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = $"[Write<{typeof(T).Name}>] Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        fixed (int* pOffsets = offsets)
        {
            _pipe.Write(PipeRequest.Read);
            _pipe.Write<WriteRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)&value,
                GetNativeSizeOf<T>()));

            PipeResponse response = _pipe.Read<PipeResponse>();
            if (response != PipeResponse.Success)
            {
                string msg = $"[Write<{typeof(T).Name}>] Failed to write value ({response} {(int)response}).";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }
        }
    }

    public override unsafe bool TryWrite<T>(T value, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[Write<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = $"[Write<{typeof(T).Name}>] Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = $"[Write<{typeof(T).Name}>] Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        fixed (int* pOffsets = offsets)
        {
            _pipe.Write(PipeRequest.Read);
            _pipe.Write<WriteRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)&value,
                GetNativeSizeOf<T>()));

            return _pipe.TryRead(out PipeResponse response) && response == PipeResponse.Success;
        }
    }

    public override unsafe void WriteSpan<T>(ReadOnlySpan<T> values, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[WriteSpan<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = $"[WriteSpan<{typeof(T).Name}>] Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = $"[WriteSpan<{typeof(T).Name}>] Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        fixed (int* pOffsets = offsets)
        fixed (T* pValues = values)
        {
            _pipe.Write(PipeRequest.Write);
            _pipe.Write<WriteRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)pValues,
                GetNativeSizeOf<T>() * (uint)values.Length));

            PipeResponse response = _pipe.Read<PipeResponse>();
            if (response != PipeResponse.Success)
            {
                string msg = $"[WriteSpan<{typeof(T).Name}>] Failed to write values ({response} {(int)response}).";
                ThrowHelper.ThrowInvalidOperationException(msg);
            }
        }
    }

    public override unsafe bool TryWriteSpan<T>(ReadOnlySpan<T> values, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = $"[Write<{typeof(T).Name}>] Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (!_pipe.IsConnected)
        {
            string msg = $"[Write<{typeof(T).Name}>] Pipe was not connected.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = $"[Write<{typeof(T).Name}>] Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        fixed (int* pOffsets = offsets)
        fixed (T* pValues = values)
        {
            _pipe.Write(PipeRequest.Read);
            _pipe.Write<WriteRequest>(new(
                baseAddress,
                (ulong)pOffsets,
                (uint)offsets.Length,
                (ulong)pValues,
                GetNativeSizeOf<T>() * (uint)values.Length));

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
