﻿namespace AslHelp.Core.Memory.IO;

public interface IMemoryWriter
{
    bool Write<T>(T value, int baseOffset, params int[] offsets) where T : unmanaged;
    bool Write<T>(T value, string module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool Write<T>(T value, Module module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool Write<T>(T value, nint baseAddress, params int[] offsets) where T : unmanaged;

    bool WriteSpan<T>(ICollection<T> values, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ICollection<T> values, string module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ICollection<T> values, Module module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ICollection<T> values, nint baseAddress, params int[] offsets) where T : unmanaged;

    bool WriteSpan<T>(List<T> values, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(List<T> values, string module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(List<T> values, Module module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(List<T> values, nint baseAddress, params int[] offsets) where T : unmanaged;

    bool WriteSpan<T>(T[] values, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(T[] values, string module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(T[] values, Module module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(T[] values, nint baseAddress, params int[] offsets) where T : unmanaged;

    bool WriteSpan<T>(ReadOnlySpan<T> values, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ReadOnlySpan<T> values, string module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ReadOnlySpan<T> values, Module module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool WriteSpan<T>(ReadOnlySpan<T> values, nint baseAddress, params int[] offsets) where T : unmanaged;
}