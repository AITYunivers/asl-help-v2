using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AslHelp.Core.Memory.Ipc;

public interface IMemoryWriter
{
    void Write<T>(T value, uint baseOffset, params int[] offsets) where T : unmanaged;
    void Write<T>(T value, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    void Write<T>(T value, Module module, uint baseOffset, params int[] offsets) where T : unmanaged;
    void Write<T>(T value, nuint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryWrite<T>(T value, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWrite<T>(T value, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWrite<T>(T value, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWrite<T>(T value, nuint baseAddress, params int[] offsets) where T : unmanaged;

    void WriteSpan<T>(ICollection<T> values, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteSpan<T>(ICollection<T> values, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteSpan<T>(ICollection<T> values, Module module, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteSpan<T>(ICollection<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryWriteSpan<T>(ICollection<T> values, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteSpan<T>(ICollection<T> values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteSpan<T>(ICollection<T> values, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteSpan<T>(ICollection<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged;

    void WriteSpan<T>(List<T> values, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteSpan<T>(List<T> values, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteSpan<T>(List<T> values, Module module, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteSpan<T>(List<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryWriteSpan<T>(List<T> values, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteSpan<T>(List<T> values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteSpan<T>(List<T> values, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteSpan<T>(List<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged;

    void WriteSpan<T>(T[] values, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteSpan<T>(T[] values, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteSpan<T>(T[] values, Module module, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteSpan<T>(T[] values, nuint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryWriteSpan<T>(T[] values, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteSpan<T>(T[] values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteSpan<T>(T[] values, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteSpan<T>(T[] values, nuint baseAddress, params int[] offsets) where T : unmanaged;

    void WriteSpan<T>(ReadOnlySpan<T> values, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteSpan<T>(ReadOnlySpan<T> values, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteSpan<T>(ReadOnlySpan<T> values, Module module, uint baseOffset, params int[] offsets) where T : unmanaged;
    void WriteSpan<T>(ReadOnlySpan<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryWriteSpan<T>(ReadOnlySpan<T> values, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteSpan<T>(ReadOnlySpan<T> values, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteSpan<T>(ReadOnlySpan<T> values, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryWriteSpan<T>(ReadOnlySpan<T> values, nuint baseAddress, params int[] offsets) where T : unmanaged;
}
