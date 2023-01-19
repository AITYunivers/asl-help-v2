﻿using AslHelp.Core.Reflection;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.IO;

public interface IMemoryReader
{
    nint Deref(int baseOffset, params int[] offsets);
    nint Deref(string module, int baseOffset, params int[] offsets);
    nint Deref(Module module, int baseOffset, params int[] offsets);
    nint Deref(nint baseAddress, params int[] offsets);

    bool TryDeref(out nint result, int baseOffset, params int[] offsets);
    bool TryDeref(out nint result, string module, int baseOffset, params int[] offsets);
    bool TryDeref(out nint result, Module module, int baseOffset, params int[] offsets);
    bool TryDeref(out nint result, nint baseAddress, params int[] offsets);

    T Read<T>(int baseOffset, params int[] offsets) where T : unmanaged;
    T Read<T>(string module, int baseOffset, params int[] offsets) where T : unmanaged;
    T Read<T>(Module module, int baseOffset, params int[] offsets) where T : unmanaged;
    T Read<T>(nint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryRead<T>(out T result, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryRead<T>(out T result, string module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryRead<T>(out T result, Module module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryRead<T>(out T result, nint baseAddress, params int[] offsets) where T : unmanaged;

    T[] ReadSpan<T>(int length, int baseOffset, params int[] offsets) where T : unmanaged;
    T[] ReadSpan<T>(int length, string module, int baseOffset, params int[] offsets) where T : unmanaged;
    T[] ReadSpan<T>(int length, Module module, int baseOffset, params int[] offsets) where T : unmanaged;
    T[] ReadSpan<T>(int length, nint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryReadSpan<T>(out T[] results, int length, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>(out T[] results, int length, string module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>(out T[] results, int length, Module module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>(out T[] results, int length, nint baseAddress, params int[] offsets) where T : unmanaged;

    bool TryReadSpan<T>(Span<T> buffer, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>(Span<T> buffer, string module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>(Span<T> buffer, Module module, int baseOffset, params int[] offsets) where T : unmanaged;
    bool TryReadSpan<T>(Span<T> buffer, nint baseAddress, params int[] offsets) where T : unmanaged;

    string ReadString(int baseOffset, params int[] offsets);
    string ReadString(int length, int baseOffset, params int[] offsets);
    string ReadString(ReadStringType stringType, int baseOffset, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, int baseOffset, params int[] offsets);

    string ReadString(string module, int baseOffset, params int[] offsets);
    string ReadString(int length, string module, int baseOffset, params int[] offsets);
    string ReadString(ReadStringType stringType, string module, int baseOffset, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets);

    string ReadString(Module module, int baseOffset, params int[] offsets);
    string ReadString(int length, Module module, int baseOffset, params int[] offsets);
    string ReadString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets);

    string ReadString(nint baseAddress, params int[] offsets);
    string ReadString(int length, nint baseAddress, params int[] offsets);
    string ReadString(ReadStringType stringType, nint baseAddress, params int[] offsets);
    string ReadString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets);

    bool TryReadString(out string result, int baseOffset, params int[] offsets);
    bool TryReadString(out string result, int length, int baseOffset, params int[] offsets);
    bool TryReadString(out string result, ReadStringType stringType, int baseOffset, params int[] offsets);
    bool TryReadString(out string result, int length, ReadStringType stringType, int baseOffset, params int[] offsets);

    bool TryReadString(out string result, string module, int baseOffset, params int[] offsets);
    bool TryReadString(out string result, int length, string module, int baseOffset, params int[] offsets);
    bool TryReadString(out string result, ReadStringType stringType, string module, int baseOffset, params int[] offsets);
    bool TryReadString(out string result, int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets);

    bool TryReadString(out string result, Module module, int baseOffset, params int[] offsets);
    bool TryReadString(out string result, int length, Module module, int baseOffset, params int[] offsets);
    bool TryReadString(out string result, ReadStringType stringType, Module module, int baseOffset, params int[] offsets);
    bool TryReadString(out string result, int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets);

    bool TryReadString(out string result, nint baseAddress, params int[] offsets);
    bool TryReadString(out string result, int length, nint baseAddress, params int[] offsets);
    bool TryReadString(out string result, ReadStringType stringType, nint baseAddress, params int[] offsets);
    bool TryReadString(out string result, int length, ReadStringType stringType, nint baseAddress, params int[] offsets);

    dynamic ReadDef(ITypeDefinition definition, int baseOffset, params int[] offsets);
    dynamic ReadDef(ITypeDefinition definition, string module, int baseOffset, params int[] offsets);
    dynamic ReadDef(ITypeDefinition definition, Module module, int baseOffset, params int[] offsets);
    dynamic ReadDef(ITypeDefinition definition, nint baseAddress, params int[] offsets);

    bool TryReadDef(ITypeDefinition definition, out dynamic result, int baseOffset, params int[] offsets);
    bool TryReadDef(ITypeDefinition definition, out dynamic result, string module, int baseOffset, params int[] offsets);
    bool TryReadDef(ITypeDefinition definition, out dynamic result, Module module, int baseOffset, params int[] offsets);
    bool TryReadDef(ITypeDefinition definition, out dynamic result, nint baseAddress, params int[] offsets);

    dynamic[] ReadSpanDef(ITypeDefinition definition, int length, int baseOffset, params int[] offsets);
    dynamic[] ReadSpanDef(ITypeDefinition definition, int length, string module, int baseOffset, params int[] offsets);
    dynamic[] ReadSpanDef(ITypeDefinition definition, int length, Module module, int baseOffset, params int[] offsets);
    dynamic[] ReadSpanDef(ITypeDefinition definition, int length, nint baseAddress, params int[] offsets);

    bool TryReadSpanDef(ITypeDefinition definition, out dynamic[] results, int length, int baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, out dynamic[] results, int length, string module, int baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, out dynamic[] results, int length, Module module, int baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, out dynamic[] results, int length, nint baseAddress, params int[] offsets);

    bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, int baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, string module, int baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, Module module, int baseOffset, params int[] offsets);
    bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, nint baseAddress, params int[] offsets);
}
