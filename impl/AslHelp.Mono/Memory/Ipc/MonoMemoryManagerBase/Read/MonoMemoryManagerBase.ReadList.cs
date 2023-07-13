using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Memory;

namespace AslHelp.Unity.Memory.Ipc;

public partial class MonoMemoryManagerBase
{
    public List<T> ReadList<T>(uint baseOffset, params int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public List<T> ReadList<T>(string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public List<T> ReadList<T>(Module module, uint baseOffset, int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public List<T> ReadList<T>(nuint address, params int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadList<T>([NotNullWhen(true)] out List<T>? results, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadList<T>([NotNullWhen(true)] out List<T>? results, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadList<T>([NotNullWhen(true)] out List<T>? results, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadList<T>([NotNullWhen(true)] out List<T>? results, nuint address, params int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }
}
