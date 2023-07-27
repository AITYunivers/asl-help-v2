using System.Diagnostics;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Mono.Memory.Ipc;

public sealed class MonoWinApiMemoryManager : MonoMemoryManagerBase
{
    private readonly WinApiMemoryManager _baseManager;

    public MonoWinApiMemoryManager(Process process)
        : base(process)
    {
        _baseManager = new(process);
    }

    public MonoWinApiMemoryManager(Process process, ILogger logger)
        : base(process, logger)
    {
        _baseManager = new(process, logger);
    }

    public override nuint Deref(nuint baseAddress, params int[] offsets)
    {
        return _baseManager.Deref(baseAddress, offsets);
    }

    public override bool TryDeref(out nuint result, nuint baseAddress, params int[] offsets)
    {
        return _baseManager.TryDeref(out result, baseAddress, offsets);
    }

    protected internal override unsafe void Read<T>(T* buffer, uint length, nuint baseAddress, params int[] offsets)
    {
        _baseManager.Read<T>(buffer, length, baseAddress, offsets);
    }

    protected internal override unsafe bool TryRead<T>(T* buffer, uint length, nuint baseAddress, params int[] offsets)
    {
        return _baseManager.TryRead<T>(buffer, length, baseAddress, offsets);
    }

    protected internal override unsafe void Write<T>(T* data, uint length, nuint baseAddress, params int[] offsets)
    {
        _baseManager.Write<T>(data, length, baseAddress, offsets);
    }

    protected internal override unsafe bool TryWrite<T>(T* data, uint length, nuint baseAddress, params int[] offsets)
    {
        return _baseManager.TryWrite<T>(data, length, baseAddress, offsets);
    }
}
