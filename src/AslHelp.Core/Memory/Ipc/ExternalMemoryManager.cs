using System;
using System.Diagnostics;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Results;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Native;

namespace AslHelp.Core.Memory.Ipc;

public class ExternalMemoryManager : MemoryManagerBase
{
    public ExternalMemoryManager(Process process)
        : base(process) { }

    public ExternalMemoryManager(Process process, ILogger logger)
        : base(process, logger) { }

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

        if (baseAddress == 0)
        {
            return new(
                IsSuccess: false,
                Throw: static () =>
                {
                    string msg = "Attempted to dereference a null pointer.";
                    ThrowHelper.ThrowInvalidOperationException(msg);
                });
        }

        nuint result = baseAddress, handle = _processHandle;
        uint ptrSize = PtrSize;

        for (int i = 0; i < offsets.Length; i++)
        {
            if (!WinInteropWrapper.ReadMemory(handle, result, &result, ptrSize) || result == 0)
            {
                return new(
                    IsSuccess: false,
                    Throw: static () =>
                    {
                        string msg = "Failed to dereference pointer.";
                        ThrowHelper.ThrowInvalidOperationException(msg);
                    });
            }

            result += (uint)offsets[i];
        }

        return new(
            IsSuccess: true,
            Value: result);
    }

    protected override unsafe Result TryRead<T>(T* buffer, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
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

        Result<nuint> derefResult = TryDeref(baseAddress, offsets);
        if (!derefResult.IsSuccess)
        {
            return derefResult;
        }

        nuint deref = derefResult.Value, handle = _processHandle;
        return new(
            IsSuccess: WinInteropWrapper.ReadMemory(handle, deref, buffer, length),
            Throw: static () =>
            {
                string msg = "Failed to read value.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            });
    }

    protected override unsafe Result TryWrite<T>(T* data, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
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

        Result<nuint> derefResult = TryDeref(baseAddress, offsets);
        if (!derefResult.IsSuccess)
        {
            return derefResult;
        }

        nuint deref = derefResult.Value, handle = _processHandle;
        return new(
            IsSuccess: WinInteropWrapper.ReadMemory(handle, deref, data, length),
            Throw: static () =>
            {
                string msg = "Failed to read value.";
                ThrowHelper.ThrowInvalidOperationException(msg);
            });
    }
}
