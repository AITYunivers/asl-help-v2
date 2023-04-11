using System.IO.Pipes;
using AslHelp.Core.IO.Logging;
using AslHelp.Core.Memory.Pipes;
using CommunityToolkit.HighPerformance;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.IO;

public sealed unsafe class PipeMemoryManager : MemoryManagerBase
{
    private readonly NamedPipeClientStream _pipe;
    private readonly nint _hModule;

    public PipeMemoryManager(Process process, NamedPipeClientStream pipe, nint hModule)
        : this(process, null, pipe, hModule) { }

    public PipeMemoryManager(Process process, ILogger logger, NamedPipeClientStream pipe, nint hModule)
        : base(process, logger)
    {
        _pipe = pipe;
        _hModule = hModule;
    }

    public bool IsConnected => _pipe.IsConnected && _pipe.CanRead && _pipe.CanWrite;

    public sealed override bool TryDeref(out nint result, nint baseAddress, params int[] offsets)
    {
        if (!IsConnected)
        {
            result = default;
            return false;
        }

        DerefRequest req = new()
        {
            Code = PipeRequestCode.Deref,
            BaseAddress = baseAddress,
            OffsetCount = offsets.Length
        };

        fixed (int* pOffsets = offsets)
        {
            Unsafe.CopyBlock(req.Offsets, pOffsets, (uint)(offsets.Length * sizeof(int)));
        }

        _pipe.Write(in req);

        PipeResponseCode code = _pipe.Read<PipeResponseCode>();
        if (code != PipeResponseCode.Success)
        {
            result = default;
            return false;
        }

        result = (nint)_pipe.Read<long>();
        return result != default;
    }

    private unsafe struct DerefRequest
    {
        public PipeRequestCode Code;
        public long BaseAddress;
        public int OffsetCount;
        public fixed int Offsets[128];
    }

    private unsafe struct ReadRequest
    {
        public PipeRequestCode Code;
        public long BaseAddress;
        public int OffsetCount;
        public fixed int Offsets[64];
        public int TypeSize;
    }

    public sealed override unsafe bool TryRead<T>(out T result, nint baseAddress, params int[] offsets)
    {
        if (!IsConnected)
        {
            result = default;
            return false;
        }

        if (baseAddress == 0)
        {
            result = default;
            return false;
        }

        ReadRequest req = new()
        {
            Code = PipeRequestCode.Deref,
            BaseAddress = baseAddress,
            OffsetCount = offsets.Length,
            TypeSize = Native.GetTypeSize<T>(Is64Bit)
        };

        fixed (int* pOffsets = offsets)
        {
            Unsafe.CopyBlock(req.Offsets, pOffsets, (uint)(offsets.Length * sizeof(int)));
        }

        _pipe.Write(in req);

        PipeResponseCode code = _pipe.Read<PipeResponseCode>();

        if (code != PipeResponseCode.Success)
        {
            result = default;
            return false;
        }

        result = _pipe.Read<T>();
        return !Native.IsPointer<T>() || !result.Equals(default(T));
    }

    public sealed override unsafe bool TryReadSpan<T>(Span<T> buffer, nint baseAddress, params int[] offsets)
    {
        if (!IsConnected)
        {
            return false;
        }

        _pipe.Write(PipeRequestCode.ReadSpan);

        _pipe.Write<long>(baseAddress);
        _pipe.Write(offsets.Length);
        _pipe.Write(MemoryMarshal.AsBytes<int>(offsets));

        _pipe.Write(Native.GetTypeSize<T>(Is64Bit) * buffer.Length);

        PipeResponseCode code = _pipe.Read<PipeResponseCode>();

        if (code != PipeResponseCode.Success)
        {
            return false;
        }

        _ = _pipe.Read(MemoryMarshal.AsBytes(buffer));
        return true;
    }

    public override bool TryReadString(out string result, int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public override bool TryReadSizedString(out string result, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
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

        _pipe.Write(Native.GetTypeSize<T>(Is64Bit));
        _pipe.Write(value);

        PipeResponseCode code = _pipe.Read<PipeResponseCode>();

        return code == PipeResponseCode.Success;
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

        _pipe.Write(Native.GetTypeSize<T>(Is64Bit) * values.Length);
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
