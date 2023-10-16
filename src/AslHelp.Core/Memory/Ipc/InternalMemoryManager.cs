using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;
using AslHelp.Common.Memory.Ipc;
using AslHelp.Common.Results;
using AslHelp.Core.Diagnostics.Logging;

namespace AslHelp.Core.Memory.Ipc;

public class InternalMemoryManager : MemoryManager
{
    [SkipLocalsInit]
    [StructLayout(LayoutKind.Explicit)]
    private unsafe struct Request
    {
        [FieldOffset(0x000)] public nuint BaseAddress;
        [FieldOffset(0x008)] public int OffsetsLength;
        [FieldOffset(0x00C)] public fixed int Offsets[128];
        [FieldOffset(0x20C)] public uint Bytes;
    }

    private readonly NamedPipeClientStream _pipe;

    public InternalMemoryManager(Process process, string namedPipeName, int timeout = -1)
        : this(process, null, namedPipeName, timeout) { }

    public InternalMemoryManager(Process process, ILogger? logger, string namedPipeName, int timeout = -1)
        : base(process, logger)
    {
        _pipe = new(namedPipeName);
        _pipe.Connect(timeout);
    }

    public InternalMemoryManager(Process process, NamedPipeClientStream pipe)
        : this(process, null, pipe) { }

    public InternalMemoryManager(Process process, ILogger? logger, NamedPipeClientStream pipe)
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

    protected internal override unsafe Result<nuint, IpcError> TryDeref(nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        if (VerifyArguments(baseAddress, offsets) is { IsSuccess: false } errorResult)
        {
            return errorResult;
        }

        Request req = new()
        {
            BaseAddress = baseAddress,
            OffsetsLength = offsets.Length
        };

        offsets.CopyTo(new(req.Offsets, offsets.Length));

        _pipe.Write(PipeRequest.Deref);
        _pipe.Write(req);

        if (!_pipe.TryRead(out PipeResponse response))
        {
            return new(
                IsSuccess: false,
                Error: IpcError.ReceiveFailure);
        }

        if (response != PipeResponse.Success)
        {
            return new(
                IsSuccess: false,
                Error: new(IpcError.DerefFailure, $"Failure during dereferencing ({(int)response}: '{response}')."));
        }

        if (!_pipe.TryRead(out ulong deref))
        {
            return new(
                IsSuccess: false,
                Error: IpcError.ReceiveFailure);
        }

        return new(
            IsSuccess: true,
            Value: (nuint)deref);
    }

    protected internal override unsafe Result<IpcError> TryRead<T>(T* buffer, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        if (VerifyArguments(baseAddress, offsets) is { IsSuccess: false } errorResult)
        {
            return errorResult;
        }

        Request req = new()
        {
            BaseAddress = baseAddress,
            OffsetsLength = offsets.Length,
            Bytes = length
        };

        offsets.CopyTo(new(req.Offsets, offsets.Length));

        _pipe.Write(PipeRequest.Read);
        _pipe.Write(req);

        if (!_pipe.TryRead(out PipeResponse response))
        {
            return new(
                IsSuccess: false,
                Error: IpcError.ReceiveFailure);
        }

        if (response != PipeResponse.Success)
        {
            return new(
                IsSuccess: false,
                Error: new(IpcError.DerefFailure, $"Failure during dereferencing ({(int)response}: '{response}')."));
        }

        if (!_pipe.TryRead<byte>(new(buffer, (int)length)))
        {
            return new(
                IsSuccess: false,
                Error: IpcError.ReceiveFailure);
        }

        return new(
            IsSuccess: true);
    }

    protected internal override unsafe Result<IpcError> TryWrite<T>(T* data, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        if (VerifyArguments(baseAddress, offsets) is { IsSuccess: false } errorResult)
        {
            return errorResult;
        }

        Request req = new()
        {
            BaseAddress = baseAddress,
            OffsetsLength = offsets.Length,
            Bytes = length
        };

        offsets.CopyTo(new(req.Offsets, offsets.Length));

        _pipe.Write(PipeRequest.Write);
        _pipe.Write(req);
        _pipe.Write(new(data, (int)length));

        if (!_pipe.TryRead(out PipeResponse response))
        {
            return new(
                IsSuccess: false,
                Error: IpcError.ReceiveFailure);
        }

        return new(
            IsSuccess: response == PipeResponse.Success,
            Error: new(IpcError.WriteFailure, $"Failure during memory write ({(int)response}: '{response}')."));
    }

    private Result<nuint, IpcError> VerifyArguments(nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        if (_isDisposed)
        {
            return new(
                IsSuccess: false,
                Error: IpcError.MemoryIsDisposed);
        }

        if (offsets.Length > 128)
        {
            return new(
                IsSuccess: false,
                Error: IpcError.OffsetsLengthIsGreaterThan128);
        }

        if (baseAddress == 0)
        {
            return new(
                IsSuccess: false,
                Error: IpcError.BaseAddressIsNullPtr);
        }

        return new(
            IsSuccess: true);
    }
}
