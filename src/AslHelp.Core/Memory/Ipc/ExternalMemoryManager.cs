using System;
using System.Diagnostics;

using AslHelp.Common.Results;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Native;

namespace AslHelp.Core.Memory.Ipc;

public class ExternalMemoryManager : MemoryManagerBase
{
    public ExternalMemoryManager(Process process)
        : this(process, null) { }

    public ExternalMemoryManager(Process process, ILogger? logger)
        : base(process, logger) { }

    protected internal override unsafe Result<nuint, IpcError> TryDeref(nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        if (_isDisposed)
        {
            return new(
                IsSuccess: false,
                Error: IpcError.MemoryIsDisposed);
        }

        if (baseAddress == 0)
        {
            return new(
                IsSuccess: false,
                Error: IpcError.BaseAddressIsNullPtr);
        }

        nuint result = baseAddress, handle = _processHandle;
        uint ptrSize = PointerSize;

        for (int i = 0; i < offsets.Length; i++)
        {
            if (!WinInteropWrapper.ReadMemory(handle, result, &result, ptrSize) || result == 0)
            {
                return new(
                    IsSuccess: false,
                    Error: IpcError.DerefFailure);
            }

            result += (uint)offsets[i];
        }

        return new(
            IsSuccess: true,
            Value: result);
    }

    protected internal override unsafe Result<IpcError> TryRead<T>(T* buffer, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        if (_isDisposed)
        {
            return new(
                IsSuccess: false,
                Error: IpcError.MemoryIsDisposed);
        }

        Result<nuint, IpcError> derefResult = TryDeref(baseAddress, offsets);
        if (!derefResult.IsSuccess)
        {
            return derefResult;
        }

        nuint deref = derefResult.Value, handle = _processHandle;
        return new(
            IsSuccess: WinInteropWrapper.ReadMemory(handle, deref, buffer, length),
            Error: IpcError.ReadFailure);
    }

    protected internal override unsafe Result<IpcError> TryWrite<T>(T* data, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        if (_isDisposed)
        {
            return new(
                IsSuccess: false,
                Error: IpcError.MemoryIsDisposed);
        }

        Result<nuint, IpcError> derefResult = TryDeref(baseAddress, offsets);
        if (!derefResult.IsSuccess)
        {
            return derefResult;
        }

        nuint deref = derefResult.Value, handle = _processHandle;
        return new(
            IsSuccess: WinInteropWrapper.WriteMemory(handle, deref, data, length),
            Error: IpcError.WriteFailure);
    }
}
