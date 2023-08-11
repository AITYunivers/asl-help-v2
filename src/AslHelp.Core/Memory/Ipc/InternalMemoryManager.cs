using System;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;
using AslHelp.Common.Results;
using AslHelp.Core.Diagnostics.Logging;

namespace AslHelp.Core.Memory.Ipc;

public class InternalMemoryManager : MemoryManagerBase
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
        : base(process)
    {
        _pipe = new(namedPipeName);
        _pipe.Connect(timeout);
    }

    public InternalMemoryManager(Process process, ILogger logger, string namedPipeName, int timeout = -1)
        : base(process, logger)
    {
        _pipe = new(namedPipeName);
        _pipe.Connect(timeout);
    }

    public InternalMemoryManager(Process process, NamedPipeClientStream pipe)
        : base(process)
    {
        VerifyPipeState(pipe);
        _pipe = pipe;
    }

    public InternalMemoryManager(Process process, ILogger logger, NamedPipeClientStream pipe)
        : base(process, logger)
    {
        VerifyPipeState(pipe);
        _pipe = pipe;
    }

    private static void VerifyPipeState(NamedPipeClientStream pipe)
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
    }

    protected internal override unsafe Result<nuint> TryDeref(nuint baseAddress, ReadOnlySpan<int> offsets)
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

        _pipe.Write(1);
        _pipe.Write(req);

        if (!_pipe.TryRead(out int response) || response != 0)
        {
            return new(
                IsSuccess: false,
                Throw: () =>
                {
                    string msg = $"Failed to dereference pointer ({(int)response}: '{response}').";
                    ThrowHelper.ThrowInvalidOperationException(msg);
                });
        }

        return new(
            IsSuccess: _pipe.TryRead(out ulong deref),
            Value: (nuint)deref,
            Throw: static () =>
            {
                const string msg = "Failed to receive result.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            });
    }

    protected internal override unsafe Result TryRead<T>(T* buffer, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
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

        _pipe.Write(2);
        _pipe.Write(req);

        if (!_pipe.TryRead(out int response) || response != 0)
        {
            return new(
                IsSuccess: false,
                Throw: () =>
                {
                    string msg = $"Failed to dereference pointer ({(int)response}: '{response}').";
                    ThrowHelper.ThrowInvalidOperationException(msg);
                });
        }

        return new(
            IsSuccess: _pipe.TryRead<byte>(new(buffer, (int)length)),
            Throw: static () =>
            {
                const string msg = "Failed to receive result.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            });
    }

    protected internal override unsafe Result TryWrite<T>(T* data, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
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

        _pipe.Write(3);
        _pipe.Write(req);
        _pipe.Write(new(data, (int)length));

        return new(
            IsSuccess: _pipe.TryRead(out int response) && response != 0,
            Throw: () =>
            {
                string msg = $"Failed to dereference pointer ({(int)response}: '{response}').";
                ThrowHelper.ThrowInvalidOperationException(msg);
            });
    }

    private Result<nuint> VerifyArguments(nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        if (_isDisposed)
        {
            return new(
                IsSuccess: false,
                Throw: static () =>
                {
                    const string msg = "Cannot interact with the memory of an exited process.";
                    ThrowHelper.ThrowInvalidOperationException(msg);
                });
        }

        if (offsets.Length > 128)
        {
            return new(
                IsSuccess: false,
                Throw: static () =>
                {
                    const string msg = "Cannot dereference more than 128 offsets.";
                    ThrowHelper.ThrowArgumentException(nameof(offsets), msg);
                });
        }

        if (baseAddress == 0)
        {
            return new(
                IsSuccess: false,
                Throw: static () =>
                {
                    const string msg = "Attempted to dereference a null pointer.";
                    ThrowHelper.ThrowArgumentException(nameof(baseAddress), msg);
                });
        }

        return new(IsSuccess: true);
    }
}
