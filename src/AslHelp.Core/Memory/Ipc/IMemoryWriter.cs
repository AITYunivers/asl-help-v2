using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AslHelp.Core.Memory.Ipc;

public interface IMemoryWriter
{
    bool Write<T>(T value, int baseOffset, params int[] offsets) where T : unmanaged;
    bool Write<T>(T value, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged;
    bool Write<T>(T value, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool Write<T>(T value, nint baseAddress, params int[] offsets) where T : unmanaged;

    bool WriteSpan<T>(ICollection<T> values, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ICollection<T> values, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ICollection<T> values, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ICollection<T> values, nint baseAddress, params int[] offsets) where T : unmanaged;

    bool WriteSpan<T>(List<T> values, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(List<T> values, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(List<T> values, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(List<T> values, nint baseAddress, params int[] offsets) where T : unmanaged;

    bool WriteSpan<T>(T[] values, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(T[] values, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(T[] values, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(T[] values, nint baseAddress, params int[] offsets) where T : unmanaged;

    bool WriteSpan<T>(ReadOnlySpan<T> values, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ReadOnlySpan<T> values, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ReadOnlySpan<T> values, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ReadOnlySpan<T> values, nint baseAddress, params int[] offsets) where T : unmanaged;
}
