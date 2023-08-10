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

    public InternalMemoryManager(Process process, ILogger logger, NamedPipeClientStream pipe)
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

    [SkipLocalsInit]
    [StructLayout(LayoutKind.Explicit)]
    private unsafe struct Request
    {
        [FieldOffset(0x000)] public nuint BaseAddress;
        [FieldOffset(0x008)] public int OffsetsLength;
        [FieldOffset(0x00C)] public fixed int Offsets[128];
        [FieldOffset(0x20C)] public uint Bytes;
    }

    protected override unsafe Result<nuint> TryDeref(nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        if (_isDisposed)
        {
            return new(
                IsSuccess: false,
                Throw: static () =>
                {
                    string msg = "Cannot interact with the memory of an exited process.";
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

    protected override unsafe Result TryRead<T>(T* buffer, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        throw new NotImplementedException();
    }

    protected override unsafe Result TryWrite<T>(T* data, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        throw new NotImplementedException();
    }
}
