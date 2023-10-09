using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Results;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public void Write<T>(T value, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Write<T>(value, MainModule.Base + baseOffset, offsets);
    }

    public void Write<T>(T value, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Write<T>(value, Modules[moduleName].Base + baseOffset, offsets);
    }

    public void Write<T>(T value, Module module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        Write<T>(value, module.Base + baseOffset, offsets);
    }

    public unsafe void Write<T>(T value, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        var writeResult = TryWrite<T>(&value, GetNativeSizeOf<T>(), baseAddress, offsets);

        if (!writeResult.IsSuccess)
        {
            ThrowHelper.ThrowException(writeResult.Error.Message);
        }
    }

    public bool TryWrite<T>(T value, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryWrite<T>(value, MainModule.Base + baseOffset, offsets);
    }

    public bool TryWrite<T>(T value, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            return false;
        }

        return TryWrite<T>(value, Modules[moduleName].Base + baseOffset, offsets);
    }

    public bool TryWrite<T>(T value, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            return false;
        }

        return TryWrite<T>(value, module.Base + baseOffset, offsets);
    }

    public unsafe bool TryWrite<T>(T value, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        var writeResult = TryWrite<T>(&value, GetNativeSizeOf<T>(), baseAddress, offsets);
        return writeResult.IsSuccess;
    }

    protected internal abstract unsafe Result<IpcError> TryWrite<T>(T* data, uint length, nuint baseAddress, ReadOnlySpan<int> offsets) where T : unmanaged;
}
