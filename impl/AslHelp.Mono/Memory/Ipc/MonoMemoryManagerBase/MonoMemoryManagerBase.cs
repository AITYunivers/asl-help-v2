using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using AslHelp.Core.Diagnostics.Logging;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Mono.Memory.Ipc;

namespace AslHelp.Unity.Memory.Ipc;

public abstract partial class MonoMemoryManagerBase : MemoryManagerBase, IMonoMemoryReader, IMonoMemoryWriter
{
    public MonoMemoryManagerBase(Process process)
        : base(process) { }

    public MonoMemoryManagerBase(Process process, ILogger logger)
        : base(process, logger) { }

    public Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(uint baseOffset, params int[] offsets)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(string moduleName, uint baseOffset, params int[] offsets)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(Module module, uint baseOffset, int[] offsets)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(nuint address, params int[] offsets)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<string, TValue> ReadDictionary<TValue>(uint baseOffset, params int[] offsets) where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<string, TValue> ReadDictionary<TValue>(string moduleName, uint baseOffset, params int[] offsets) where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<string, TValue> ReadDictionary<TValue>(Module module, uint baseOffset, int[] offsets) where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<string, TValue> ReadDictionary<TValue>(nuint address, params int[] offsets) where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public HashSet<T> ReadHashSet<T>(uint baseOffset, params int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public HashSet<T> ReadHashSet<T>(string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public HashSet<T> ReadHashSet<T>(Module module, uint baseOffset, int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public HashSet<T> ReadHashSet<T>(nuint address, params int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

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

    public string ReadString(uint baseOffset, params int[] offsets)
    {
        throw new System.NotImplementedException();
    }

    public string ReadString(string moduleName, uint baseOffset, params int[] offsets)
    {
        throw new System.NotImplementedException();
    }

    public string ReadString(Module module, uint baseOffset, int[] offsets)
    {
        throw new System.NotImplementedException();
    }

    public string ReadString(nuint address, params int[] offsets)
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadDictionary<TKey, TValue>([NotNullWhen(true)] out Dictionary<TKey, TValue>? results, uint baseOffset, params int[] offsets)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadDictionary<TKey, TValue>([NotNullWhen(true)] out Dictionary<TKey, TValue>? results, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadDictionary<TKey, TValue>([NotNullWhen(true)] out Dictionary<TKey, TValue>? results, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadDictionary<TKey, TValue>([NotNullWhen(true)] out Dictionary<TKey, TValue>? results, nuint address, params int[] offsets)
        where TKey : unmanaged
        where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadDictionary<TValue>([NotNullWhen(true)] out Dictionary<string, TValue>? results, uint baseOffset, params int[] offsets) where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadDictionary<TValue>([NotNullWhen(true)] out Dictionary<string, TValue>? results, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadDictionary<TValue>([NotNullWhen(true)] out Dictionary<string, TValue>? results, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets) where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadDictionary<TValue>([NotNullWhen(true)] out Dictionary<string, TValue>? results, nuint address, params int[] offsets) where TValue : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadHashSet<T>([NotNullWhen(true)] out HashSet<T>? results, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadHashSet<T>([NotNullWhen(true)] out HashSet<T>? results, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadHashSet<T>([NotNullWhen(true)] out HashSet<T>? results, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets) where T : unmanaged
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadHashSet<T>([NotNullWhen(true)] out HashSet<T>? results, nuint address, params int[] offsets) where T : unmanaged
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

    public bool TryReadString([NotNullWhen(true)] out string? result, uint baseOffset, params int[] offsets)
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets)
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets)
    {
        throw new System.NotImplementedException();
    }

    public bool TryReadString([NotNullWhen(true)] out string? result, nuint address, params int[] offsets)
    {
        throw new System.NotImplementedException();
    }
}
