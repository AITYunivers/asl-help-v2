using System;
using System.Diagnostics;

using AslHelp.Common.Results;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Unity.Memory.Ipc;

public sealed class ExternalMonoMemoryManager : MonoMemoryManagerBase
{
    private readonly ExternalMemoryManager _baseManager;

    public ExternalMonoMemoryManager(Process process)
        : base(process)
    {
        _baseManager = new(process);
    }

    public ExternalMonoMemoryManager(Process process, ILogger logger)
        : base(process, logger)
    {
        _baseManager = new(process, logger);
    }

    protected internal override Result<nuint, IpcError> TryDeref(nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        return _baseManager.TryDeref(baseAddress, offsets);
    }

    protected internal override unsafe Result<IpcError> TryRead<T>(T* buffer, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        return _baseManager.TryRead(buffer, length, baseAddress, offsets);
    }

    protected internal override unsafe Result<IpcError> TryWrite<T>(T* data, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        return _baseManager.TryWrite(data, length, baseAddress, offsets);
    }
}
