using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AslHelp.Core.Memory.Ipc;

public interface IMemoryWriter
{
    bool Write<T>(T value, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool Write<T>(T value, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool Write<T>(T value, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool Write<T>(T value, nuint baseAddress, params int[] offsets) where T : unmanaged;

    bool WriteSpan<T>(ICollection<T> values, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ICollection<T> values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ICollection<T> values, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ICollection<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged;

    bool WriteSpan<T>(List<T> values, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(List<T> values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(List<T> values, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(List<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged;

    bool WriteSpan<T>(T[] values, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(T[] values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(T[] values, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(T[] values, nuint baseAddress, params int[] offsets) where T : unmanaged;

    bool WriteSpan<T>(ReadOnlySpan<T> values, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ReadOnlySpan<T> values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ReadOnlySpan<T> values, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ReadOnlySpan<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged;
}
