﻿using System;
using System.Diagnostics.CodeAnalysis;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Ipc;

public interface IMemoryReader
{
    nuint Deref(uint baseOffset, params int[] offsets);
    nuint Deref(string moduleName, uint baseOffset, params int[] offsets);
    nuint Deref(Module module, uint baseOffset, params int[] offsets);
    nuint Deref(nuint baseAddress, params int[] offsets);

    bool TryDeref(out nuint result, uint baseOffset, params int[] offsets);
    bool TryDeref(out nuint result, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets);
    bool TryDeref(out nuint result, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets);
    bool TryDeref(out nuint result, nuint baseAddress, params int[] offsets);

    T Read<T>(uint baseOffset, params int[] offsets) where T : unmanaged;
    T Read<T>(string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    T Read<T>(Module module, uint baseOffset, params int[] offsets) where T : unmanaged;
    T Read<T>(nuint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryRead<T>(out T result, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryRead<T>(out T result, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryRead<T>(out T result, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryRead<T>(out T result, nuint baseAddress, params int[] offsets) where T : unmanaged;

    T[] ReadSpan<T>(int length, uint baseOffset, params int[] offsets) where T : unmanaged;
    T[] ReadSpan<T>(int length, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    T[] ReadSpan<T>(int length, Module module, uint baseOffset, params int[] offsets) where T : unmanaged;
    T[] ReadSpan<T>(int length, nuint baseAddress, params int[] offsets) where T : unmanaged;

    void ReadSpan<T>(Span<T> buffer, uint baseOffset, params int[] offsets) where T : unmanaged;
    void ReadSpan<T>(Span<T> buffer, string moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    void ReadSpan<T>(Span<T> buffer, Module module, uint baseOffset, params int[] offsets) where T : unmanaged;
    void ReadSpan<T>(Span<T> buffer, nuint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, nuint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryReadSpan<T>(Span<T> buffer, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>(Span<T> buffer, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>(Span<T> buffer, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>(Span<T> buffer, nuint baseAddress, params int[] offsets) where T : unmanaged;

    string ReadString(int length, uint baseOffset, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, uint baseOffset, params int[] offsets);

    string ReadString(int length, string moduleName, uint baseOffset, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets);

    string ReadString(int length, Module module, uint baseOffset, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, Module module, uint baseOffset, params int[] offsets);

    string ReadString(int length, nuint baseAddress, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, nuint baseAddress, params int[] offsets);

    bool TryReadString([NotNullWhen(true)] out string? result, int length, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, uint baseOffset, params int[] offsets);

    bool TryReadString([NotNullWhen(true)] out string? result, int length, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets);

    bool TryReadString([NotNullWhen(true)] out string? result, int length, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets);

    bool TryReadString([NotNullWhen(true)] out string? result, int length, nuint baseAddress, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, nuint baseAddress, params int[] offsets);

    string ReadSizedString(uint baseOffset, params int[] offsets);
    string ReadSizedString(ReadStringType stringType, uint baseOffset, params int[] offsets);

    string ReadSizedString(string moduleName, uint baseOffset, params int[] offsets);
    string ReadSizedString(ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets);

    string ReadSizedString(Module module, uint baseOffset, params int[] offsets);
    string ReadSizedString(ReadStringType stringType, Module module, uint baseOffset, params int[] offsets);

    string ReadSizedString(nuint baseAddress, params int[] offsets);
    string ReadSizedString(ReadStringType stringType, nuint baseAddress, params int[] offsets);

    bool TryReadSizedString([NotNullWhen(true)] out string? result, uint baseOffset, params int[] offsets);
    bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, uint baseOffset, params int[] offsets);

    bool TryReadSizedString([NotNullWhen(true)] out string? result, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets);
    bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets);

    bool TryReadSizedString([NotNullWhen(true)] out string? result, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets);
    bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets);

    bool TryReadSizedString([NotNullWhen(true)] out string? result, nuint baseAddress, params int[] offsets);
    bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, nuint baseAddress, params int[] offsets);
}
