using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Memory;

namespace AslHelp.Mono.Memory.Ipc;

public interface IMonoMemoryWriter
{
    void WriteArray<T>(T[] values, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteArray<T>(T[] values, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteArray<T>(T[] values, Module module, uint baseOffset, int[] offsets) where T : unmanaged;
    void WriteArray<T>(T[] values, nuint address, params int[] offsets) where T : unmanaged;

    bool TryWriteArray<T>(T[] values, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteArray<T>(T[] values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteArray<T>(T[] values, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets) where T : unmanaged;
    bool TryWriteArray<T>(T[] values, nuint address, params int[] offsets) where T : unmanaged;

    void WriteList<T>(List<T> values, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteList<T>(List<T> values, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteList<T>(List<T> values, Module module, uint baseOffset, int[] offsets) where T : unmanaged;
    void WriteList<T>(List<T> values, nuint address, params int[] offsets) where T : unmanaged;

    bool TryWriteList<T>(List<T> values, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteList<T>(List<T> values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteList<T>(List<T> values, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets) where T : unmanaged;
    bool TryWriteList<T>(List<T> values, nuint address, params int[] offsets) where T : unmanaged;

    // void WriteHashSet<T>(HashSet<T> values, uint baseOffset, params int[] offsets) where T : unmanaged;
    // void WriteHashSet<T>(HashSet<T> values, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    // void WriteHashSet<T>(HashSet<T> values, Module module, uint baseOffset, int[] offsets) where T : unmanaged;
    // void WriteHashSet<T>(HashSet<T> values, nuint address, params int[] offsets) where T : unmanaged;

    // bool TryWriteHashSet<T>(HashSet<T> values, uint baseOffset, params int[] offsets) where T : unmanaged;
    // bool TryWriteHashSet<T>(HashSet<T> values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    // bool TryWriteHashSet<T>(HashSet<T> values, [NotNullWhen(true)] Module? module, uint baseOffset, int[] offsets) where T : unmanaged;
    // bool TryWriteHashSet<T>(HashSet<T> values, nuint address, params int[] offsets) where T : unmanaged;
}
