using System;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;
using AslHelp.Common.Memory.Ipc;
using AslHelp.Core.Diagnostics.Logging;

namespace AslHelp.Core.Memory.Ipc;

public class InternalMemoryManager : MemoryManagerBase
{
    private readonly MemoryMappedFile _mmf;
    private readonly MemoryMappedViewAccessor _accessor;

    public InternalMemoryManager(Process process, string mappedFileName)
        : base(process)
    {
        _mmf = MemoryMappedFile.CreateOrOpen(mappedFileName, 512, MemoryMappedFileAccess.ReadWrite);
        _accessor = _mmf.CreateViewAccessor();
    }

    public InternalMemoryManager(Process process, ILogger logger, string mappedFileName)
        : base(process, logger)
    {
        _mmf = MemoryMappedFile.CreateOrOpen(mappedFileName, 512, MemoryMappedFileAccess.ReadWrite);
        _accessor = _mmf.CreateViewAccessor();
    }

    public override unsafe nuint Deref(nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = "Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        S s = default;
        s.Request = PipeRequest.Deref;
        s.BaseAddress = baseAddress;
        s.OffsetCount = (uint)offsets.Length;
        s.Length = 0;

        offsets.CopyTo(new Span<int>(s.Offsets, offsets.Length));

        for (int i = 0; i < offsets.Length; i++)
        {
            s.Offsets[i] = offsets[i];
        }

        using var handle = _accessor.SafeMemoryMappedViewHandle;

        byte* ptr = null;
        handle.AcquirePointer(ref ptr);

        fixed (int* pOffsets = offsets)
        {
            *((PipeRequest*)ptr + 0) = PipeRequest.Deref;
            *((ulong*)ptr + 4) = baseAddress;
            *((uint*)ptr + 12) = (uint)offsets.Length;
        }

        handle.Write(0, PipeRequest.Deref);

        handle.Write(8, (ulong)baseAddress);
        handle.Write(16, (uint)offsets.Length);
        handle.Write(24, offsets, 0, offsets.Length);

        _accessor.Flush();

        _view.Write(PipeRequest.Deref);

        _view.Write((ulong)baseAddress);
        _view.Write((uint)offsets.Length);
        _view.Write(MemoryMarshal.AsBytes<int>(offsets));

        PipeResponse response = _view.Read<PipeResponse>();
        if (response != PipeResponse.Success)
        {
            string msg = $"Failed to dereference pointer ({(int)response}: '{response}').";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        nuint result = (nuint)_view.Read<ulong>();

        _view.Flush();

        return result;
    }

    public override unsafe bool TryDeref(out nuint result, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            _view.Flush();

            result = default;
            return false;
        }

        _view.Write(PipeRequest.Deref);

        _view.Write((ulong)baseAddress);
        _view.Write((uint)offsets.Length);
        _view.Write(MemoryMarshal.AsBytes<int>(offsets));

        if (!_view.TryRead(out PipeResponse response) || response != PipeResponse.Success)
        {
            _view.Flush();

            result = default;
            return false;
        }

        if (!_view.TryRead(out ulong value))
        {
            _view.Flush();

            result = default;
            return false;
        }

        result = (nuint)value;

        _view.Flush();

        return true;
    }

    protected internal override unsafe void Read<T>(T* buffer, uint length, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = "Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        _view.Write(PipeRequest.Read);

        _view.Write((ulong)baseAddress);
        _view.Write((uint)offsets.Length);
        _view.Write(MemoryMarshal.AsBytes<int>(offsets));
        _view.Write(length);

        PipeResponse response = _view.Read<PipeResponse>();
        if (response != PipeResponse.Success)
        {
            string msg = $"Failed to read value ({(int)response}: '{response}').";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        Span<byte> bytes = new(buffer, (int)length);
        _view.Read(bytes);

        _view.Flush();
    }

    protected internal override unsafe bool TryRead<T>(T* buffer, uint length, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            _view.Flush();
            return false;
        }

        _view.Write(PipeRequest.Read);

        _view.Write((ulong)baseAddress);
        _view.Write((uint)offsets.Length);
        _view.Write(MemoryMarshal.AsBytes<int>(offsets));
        _view.Write(length);

        if (!_view.TryRead(out PipeResponse response) || response != PipeResponse.Success)
        {
            _view.Flush();
            return false;
        }

        if (!_view.TryRead(out T value))
        {
            _view.Flush();
            return false;
        }

        *buffer = value;

        _view.Flush();

        return true;
    }

    protected internal override unsafe void Write<T>(T* data, uint length, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = "Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        _view.Write(PipeRequest.Write);

        _view.Write((ulong)baseAddress);
        _view.Write((uint)offsets.Length);
        _view.Write(MemoryMarshal.AsBytes<int>(offsets));
        _view.Write(length);
        _view.Write(*data);

        PipeResponse response = _view.Read<PipeResponse>();
        if (response != PipeResponse.Success)
        {
            string msg = $"Failed to write value ({(int)response}: '{response}').";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        _view.Flush();
    }

    protected internal override unsafe bool TryWrite<T>(T* data, uint length, nuint baseAddress, params int[] offsets)
    {
        if (_isDisposed)
        {
            string msg = "Cannot interact with the memory of an exited process.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        if (baseAddress == 0)
        {
            string msg = "Attempted to dereference a null pointer.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        _view.Write(PipeRequest.Write);

        _view.Write((ulong)baseAddress);
        _view.Write((uint)offsets.Length);
        _view.Write(MemoryMarshal.AsBytes<int>(offsets));
        _view.Write(length);
        _view.Write(*data);

        if (!_view.TryRead(out PipeResponse response) || response != PipeResponse.Success)
        {
            _view.Flush();
            return false;
        }

        _view.Flush();
    }

    public override void Dispose()
    {
        base.Dispose();

        _pipe.Write(PipeRequest.Close);
        _pipe.Dispose();
    }
}
