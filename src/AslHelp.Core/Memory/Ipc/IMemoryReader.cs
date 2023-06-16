using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Core.Reflection;

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

    string ReadString(uint baseOffset, params int[] offsets);
    string ReadString(int length, uint baseOffset, params int[] offsets);
    string ReadString(ReadStringType stringType, uint baseOffset, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, uint baseOffset, params int[] offsets);

    string ReadString(string moduleName, uint baseOffset, params int[] offsets);
    string ReadString(int length, string moduleName, uint baseOffset, params int[] offsets);
    string ReadString(ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets);

    string ReadString(Module module, uint baseOffset, params int[] offsets);
    string ReadString(int length, Module module, uint baseOffset, params int[] offsets);
    string ReadString(ReadStringType stringType, Module module, uint baseOffset, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, Module module, uint baseOffset, params int[] offsets);

    string ReadString(nuint baseAddress, params int[] offsets);
    string ReadString(int length, nuint baseAddress, params int[] offsets);
    string ReadString(ReadStringType stringType, nuint baseAddress, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, nuint baseAddress, params int[] offsets);

    bool TryReadString([NotNullWhen(true)] out string? result, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, ReadStringType stringType, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, uint baseOffset, params int[] offsets);

    bool TryReadString([NotNullWhen(true)] out string? result, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, ReadStringType stringType, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets);

    bool TryReadString([NotNullWhen(true)] out string? result, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, ReadStringType stringType, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, ReadStringType stringType, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets);

    bool TryReadString([NotNullWhen(true)] out string? result, nuint baseAddress, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, int length, nuint baseAddress, params int[] offsets);
    bool TryReadString([NotNullWhen(true)] out string? result, ReadStringType stringType, nuint baseAddress, params int[] offsets);
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

    dynamic? ReadDef(ITypeDefinition definition, uint baseOffset, params int[] offsets);
    dynamic? ReadDef(ITypeDefinition definition, string moduleName, uint baseOffset, params int[] offsets);
    dynamic? ReadDef(ITypeDefinition definition, Module module, uint baseOffset, params int[] offsets);
    dynamic? ReadDef(ITypeDefinition definition, nuint baseAddress, params int[] offsets);

    bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, uint baseOffset, params int[] offsets);
    bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets);
    bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets);
    bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, nuint baseAddress, params int[] offsets);

    dynamic?[] ReadSpanDef(ITypeDefinition definition, int length, uint baseOffset, params int[] offsets);
    dynamic?[] ReadSpanDef(ITypeDefinition definition, int length, string moduleName, uint baseOffset, params int[] offsets);
    dynamic?[] ReadSpanDef(ITypeDefinition definition, int length, Module module, uint baseOffset, params int[] offsets);
    dynamic?[] ReadSpanDef(ITypeDefinition definition, int length, nuint baseAddress, params int[] offsets);

    void ReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, uint baseOffset, params int[] offsets);
    void ReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, string moduleName, uint baseOffset, params int[] offsets);
    void ReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, Module module, uint baseOffset, params int[] offsets);
    void ReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, nuint baseAddress, params int[] offsets);

    bool TryReadSpanDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic[]? results, int length, uint baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic[]? results, int length, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic[]? results, int length, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic[]? results, int length, nuint baseAddress, params int[] offsets);

    bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, uint baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, nuint baseAddress, params int[] offsets);
}
