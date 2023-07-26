using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
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
        Read<T>(&result, GetNativeSizeOf<T>(), baseAddress, offsets);

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
            return TryRead(pResult, GetNativeSizeOf<T>(), baseAddress, offsets);
        }
    }
}
