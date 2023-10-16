using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Results;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManager
{
    public T Read<T>(uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return Read<T>(MainModule.Base + baseOffset, offsets);
    }

    public T Read<T>(string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return Read<T>(Modules[moduleName].Base + baseOffset, offsets);
    }

    public T Read<T>(Module module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return Read<T>(module.Base + baseOffset, offsets);
    }

    public unsafe T Read<T>(nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        T result;
        var readResult = TryRead<T>(&result, GetNativeSizeOf<T>(), baseAddress, offsets);

        if (!readResult.IsSuccess)
        {
            ThrowHelper.ThrowException(readResult.Error.Message);
        }

        return result;
    }

    public bool TryRead<T>(out T result, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        return TryRead(out result, MainModule.Base + baseOffset, offsets);
    }

    public bool TryRead<T>(out T result, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (moduleName is null)
        {
            result = default;
            return false;
        }

        return TryRead(out result, Modules[moduleName].Base + baseOffset, offsets);
    }

    public bool TryRead<T>(out T result, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        if (module is null)
        {
            result = default;
            return false;
        }

        return TryRead(out result, module.Base + baseOffset, offsets);
    }

    public unsafe bool TryRead<T>(out T result, nuint baseAddress, params int[] offsets) where T : unmanaged
    {
        fixed (T* pResult = &result)
        {
            var readResult = TryRead<T>(pResult, GetNativeSizeOf<T>(), baseAddress, offsets);
            return readResult.IsSuccess;
        }
    }

    protected internal abstract unsafe Result<IpcError> TryRead<T>(T* buffer, uint length, nuint baseAddress, ReadOnlySpan<int> offsets) where T : unmanaged;
}
