using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Reflection;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Ipc;

public interface IMemoryReader
{
    nint Deref(int baseOffset, params int[] offsets);
    nint Deref(string moduleName, int baseOffset, params int[] offsets);
    nint Deref(Module module, int baseOffset, params int[] offsets);
    nint Deref(nint baseAddress, params int[] offsets);

    bool TryDeref(out nint result, int baseOffset, params int[] offsets);
    bool TryDeref(out nint result, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets);
    bool TryDeref(out nint result, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets);
    bool TryDeref(out nint result, nint baseAddress, params int[] offsets);

    T Read<T>(int baseOffset, params int[] offsets) where T : unmanaged;
    T Read<T>(string moduleName, int baseOffset, params int[] offsets) where T : unmanaged;
    T Read<T>(Module module, int baseOffset, params int[] offsets) where T : unmanaged;
    T Read<T>(nint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryRead<T>(out T result, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryRead<T>(out T result, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryRead<T>(out T result, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryRead<T>(out T result, nint baseAddress, params int[] offsets) where T : unmanaged;

    T[] ReadSpan<T>(int length, int baseOffset, params int[] offsets) where T : unmanaged;
    T[] ReadSpan<T>(int length, string moduleName, int baseOffset, params int[] offsets) where T : unmanaged;
    T[] ReadSpan<T>(int length, Module module, int baseOffset, params int[] offsets) where T : unmanaged;
    T[] ReadSpan<T>(int length, nint baseAddress, params int[] offsets) where T : unmanaged;

    void ReadSpan<T>(Span<T> buffer, int baseOffset, params int[] offsets) where T : unmanaged;
    void ReadSpan<T>(Span<T> buffer, string moduleName, int baseOffset, params int[] offsets) where T : unmanaged;
    void ReadSpan<T>(Span<T> buffer, Module module, int baseOffset, params int[] offsets) where T : unmanaged;
    void ReadSpan<T>(Span<T> buffer, nint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>([NotNullWhen(true)] out T[]? results, int length, nint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryReadSpan<T>(Span<T> buffer, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>(Span<T> buffer, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>(Span<T> buffer, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>(Span<T> buffer, nint baseAddress, params int[] offsets) where T : unmanaged;

    string ReadString(int baseOffset, params int[] offsets);
    string ReadString(int length, int baseOffset, params int[] offsets);
    string ReadString(ReadStringType stringType, int baseOffset, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, int baseOffset, params int[] offsets);

    string ReadString(string moduleName, int baseOffset, params int[] offsets);
    string ReadString(int length, string moduleName, int baseOffset, params int[] offsets);
    string ReadString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets);

    string ReadString(Module module, int baseOffset, params int[] offsets);
    string ReadString(int length, Module module, int baseOffset, params int[] offsets);
    string ReadString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets);

    string ReadString(nint baseAddress, params int[] offsets);
    string ReadString(int length, nint baseAddress, params int[] offsets);
    string ReadString(ReadStringType stringType, nint baseAddress, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets);

    bool TryReadString([NotNullWhen(true)] out string? result, int baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, int baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, ReadStringType stringType, int baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, int baseOffset, params int[] offsets);

    bool TryReadString([NotNullWhen(true)] out string? result, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, ReadStringType stringType, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets);

    bool TryReadString([NotNullWhen(true)] out string? result, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, ReadStringType stringType, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets);

    bool TryReadString([NotNullWhen(true)] out string? result, nint baseAddress, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, nint baseAddress, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, ReadStringType stringType, nint baseAddress, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, nint baseAddress, params int[] offsets);

    string ReadSizedString(int baseOffset, params int[] offsets);
    string ReadSizedString(ReadStringType stringType, int baseOffset, params int[] offsets);

    string ReadSizedString(string moduleName, int baseOffset, params int[] offsets);
    string ReadSizedString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets);

    string ReadSizedString(Module module, int baseOffset, params int[] offsets);
    string ReadSizedString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets);

    string ReadSizedString(nint baseAddress, params int[] offsets);
    string ReadSizedString(ReadStringType stringType, nint baseAddress, params int[] offsets);

    bool TryReadSizedString([NotNullWhen(true)] out string? result, int baseOffset, params int[] offsets);
    bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, int baseOffset, params int[] offsets);

    bool TryReadSizedString([NotNullWhen(true)] out string? result, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets);
    bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets);

    bool TryReadSizedString([NotNullWhen(true)] out string? result, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets);
    bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets);

    bool TryReadSizedString([NotNullWhen(true)] out string? result, nint baseAddress, params int[] offsets);
    bool TryReadSizedString([NotNullWhen(true)] out string? result, ReadStringType stringType, nint baseAddress, params int[] offsets);

    dynamic? ReadDef(ITypeDefinition definition, int baseOffset, params int[] offsets);
    dynamic? ReadDef(ITypeDefinition definition, string moduleName, int baseOffset, params int[] offsets);
    dynamic? ReadDef(ITypeDefinition definition, Module module, int baseOffset, params int[] offsets);
    dynamic? ReadDef(ITypeDefinition definition, nint baseAddress, params int[] offsets);

    bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, int baseOffset, params int[] offsets);
    bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets);
    bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets);
    bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, nint baseAddress, params int[] offsets);

    dynamic?[] ReadSpanDef(ITypeDefinition definition, int length, int baseOffset, params int[] offsets);
    dynamic?[] ReadSpanDef(ITypeDefinition definition, int length, string moduleName, int baseOffset, params int[] offsets);
    dynamic?[] ReadSpanDef(ITypeDefinition definition, int length, Module module, int baseOffset, params int[] offsets);
    dynamic?[] ReadSpanDef(ITypeDefinition definition, int length, nint baseAddress, params int[] offsets);

    void ReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, int baseOffset, params int[] offsets);
    void ReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, string moduleName, int baseOffset, params int[] offsets);
    void ReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, Module module, int baseOffset, params int[] offsets);
    void ReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, nint baseAddress, params int[] offsets);

    bool TryReadSpanDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic[]? results, int length, int baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic[]? results, int length, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic[]? results, int length, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic[]? results, int length, nint baseAddress, params int[] offsets);

    bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, int baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, [MaybeNullWhen(false)] string? moduleName, int baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, [MaybeNullWhen(false)] Module? module, int baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, nint baseAddress, params int[] offsets);
}
