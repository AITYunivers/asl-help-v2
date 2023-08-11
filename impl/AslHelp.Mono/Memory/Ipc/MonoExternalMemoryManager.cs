using System;
using System.Diagnostics;

using AslHelp.Common.Results;
using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Mono.Memory.Ipc;

public sealed class MonoExternalMemoryManager : MonoMemoryManagerBase
{
    private readonly ExternalMemoryManager _baseManager;

    public MonoExternalMemoryManager(Process process)
        : base(process)
    {
        _baseManager = new(process);
    }

    public MonoExternalMemoryManager(Process process, ILogger logger)
        : base(process, logger)
    {
        _baseManager = new(process, logger);
    }

    protected internal override unsafe Result TryRead<T>(T* buffer, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        return _baseManager.TryRead(buffer, length, baseAddress, offsets);
    }

    protected internal override unsafe Result TryWrite<T>(T* data, uint length, nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        return _baseManager.TryWrite(data, length, baseAddress, offsets);
    }

    protected internal override Result<nuint> TryDeref(nuint baseAddress, ReadOnlySpan<int> offsets)
    {
        return _baseManager.TryDeref(baseAddress, offsets);
    }
}
