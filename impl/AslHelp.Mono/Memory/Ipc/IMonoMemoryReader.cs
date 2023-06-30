using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace AslHelp.Mono.Memory.Ipc;

public interface IMonoMemoryReader
{
    T[] ReadArray<T>(uint baseOffset, params int[] offsets) where T : unmanaged;
    T[] ReadArray<T>(string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    T[] ReadArray<T>(Module module, uint baseOffset, int[] offsets) where T : unmanaged;
    T[] ReadArray<T>(nuint address, params int[] offsets) where T : unmanaged;

    bool TryReadArray<T>([NotNullWhen(true)] out T[]? results, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadArray<T>([NotNullWhen(true)] out T[]? results, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadArray<T>([NotNullWhen(true)] out T[]? results, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets) where T : unmanaged;
    bool TryReadArray<T>([NotNullWhen(true)] out T[]? results, nuint address, params int[] offsets) where T : unmanaged;

    List<T> ReadList<T>(uint baseOffset, params int[] offsets) where T : unmanaged;
    List<T> ReadList<T>(string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    List<T> ReadList<T>(Module module, uint baseOffset, int[] offsets) where T : unmanaged;
    List<T> ReadList<T>(nuint address, params int[] offsets) where T : unmanaged;

    bool TryReadList<T>([NotNullWhen(true)] out List<T>? results, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadList<T>([NotNullWhen(true)] out List<T>? results, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadList<T>([NotNullWhen(true)] out List<T>? results, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets) where T : unmanaged;
    bool TryReadList<T>([NotNullWhen(true)] out List<T>? results, nuint address, params int[] offsets) where T : unmanaged;

    HashSet<T> ReadHashSet<T>(uint baseOffset, params int[] offsets) where T : unmanaged;
    HashSet<T> ReadHashSet<T>(string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    HashSet<T> ReadHashSet<T>(Module module, uint baseOffset, int[] offsets) where T : unmanaged;
    HashSet<T> ReadHashSet<T>(nuint address, params int[] offsets) where T : unmanaged;

    bool TryReadHashSet<T>([NotNullWhen(true)] out HashSet<T>? results, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadHashSet<T>([NotNullWhen(true)] out HashSet<T>? results, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadHashSet<T>([NotNullWhen(true)] out HashSet<T>? results, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets) where T : unmanaged;
    bool TryReadHashSet<T>([NotNullWhen(true)] out HashSet<T>? results, nuint address, params int[] offsets) where T : unmanaged;

    Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(uint baseOffset, params int[] offsets) where TKey : unmanaged where TValue : unmanaged;
    Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(string moduleName, uint baseOffset, params int[] offsets) where TKey : unmanaged where TValue : unmanaged;
    Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(Module module, uint baseOffset, int[] offsets) where TKey : unmanaged where TValue : unmanaged;
    Dictionary<TKey, TValue> ReadDictionary<TKey, TValue>(nuint address, params int[] offsets) where TKey : unmanaged where TValue : unmanaged;

    bool TryReadDictionary<TKey, TValue>([NotNullWhen(true)] out Dictionary<TKey, TValue>? results, uint baseOffset, params int[] offsets) where TKey : unmanaged where TValue : unmanaged;
    bool TryReadDictionary<TKey, TValue>([NotNullWhen(true)] out Dictionary<TKey, TValue>? results, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where TKey : unmanaged where TValue : unmanaged;
    bool TryReadDictionary<TKey, TValue>([NotNullWhen(true)] out Dictionary<TKey, TValue>? results, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets) where TKey : unmanaged where TValue : unmanaged;
    bool TryReadDictionary<TKey, TValue>([NotNullWhen(true)] out Dictionary<TKey, TValue>? results, nuint address, params int[] offsets) where TKey : unmanaged where TValue : unmanaged;

    string ReadString(uint baseOffset, params int[] offsets);
    string ReadString(string moduleName, uint baseOffset, params int[] offsets);
    string ReadString(Module module, uint baseOffset, int[] offsets);
    string ReadString(nuint address, params int[] offsets);

    bool TryReadString([NotNullWhen(true)] out string? result, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, nuint address, params int[] offsets);
}
