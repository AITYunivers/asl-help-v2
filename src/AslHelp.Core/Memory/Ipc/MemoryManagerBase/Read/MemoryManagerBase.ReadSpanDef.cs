using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;
using AslHelp.Common.Extensions;
using AslHelp.Core.Reflection;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public dynamic[] ReadSpanDef(ITypeDefinition definition, int length, uint baseOffset, params int[] offsets)
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = "[ReadSpanDef] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ReadSpanDef(definition, length, module, baseOffset, offsets);
    }

    public dynamic[] ReadSpanDef(ITypeDefinition definition, int length, string moduleName, uint baseOffset, params int[] offsets)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ReadSpanDef] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ReadSpanDef(definition, length, module, baseOffset, offsets);
    }

    public dynamic[] ReadSpanDef(ITypeDefinition definition, int length, Module module, uint baseOffset, params int[] offsets)
    {
        return ReadSpanDef(definition, length, module.Base + baseOffset, offsets);
    }

    public dynamic[] ReadSpanDef(ITypeDefinition definition, int length, nuint baseAddress, params int[] offsets)
    {
        ThrowHelper.ThrowIfLessThan(length, 0);

        dynamic[] results = new dynamic[length];
        ReadSpanDef(definition, results, baseAddress, offsets);

        return results;
    }

    public void ReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, uint baseOffset, params int[] offsets)
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = "[ReadSpanDef] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        ReadSpanDef(definition, buffer, module, baseOffset, offsets);
    }

    public void ReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, string moduleName, uint baseOffset, params int[] offsets)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ReadSpanDef] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        ReadSpanDef(definition, buffer, module, baseOffset, offsets);
    }

    public void ReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, Module module, uint baseOffset, params int[] offsets)
    {
        ReadSpanDef(definition, buffer, module.Base + baseOffset, offsets);
    }

    public unsafe void ReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, nuint baseAddress, params int[] offsets)
    {
        uint size = definition.Size;
        long bytes = size * buffer.Length;

        byte[]? rented = null;
        Span<byte> bBuffer =
            bytes <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPool<byte>.Shared.Rent((int)bytes));

        ReadSpan(bBuffer, baseAddress, offsets);

        fixed (byte* pBuffer = bBuffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = definition.CreateInstance(pBuffer + (i * size));
            }
        }

        ArrayPool<byte>.Shared.ReturnIfNotNull(rented);
    }

    public bool TryReadSpanDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic[]? results, int length, uint baseOffset, params int[] offsets)
    {
        return TryReadSpanDef(definition, out results, length, MainModule, baseOffset, offsets);
    }

    public bool TryReadSpanDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic[]? results, int length, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets)
    {
        if (moduleName is null)
        {
            results = default;
            return false;
        }

        return TryReadSpanDef(definition, out results, length, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadSpanDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic[]? results, int length, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            results = default;
            return false;
        }

        return TryReadSpanDef(definition, out results, length, module.Base + baseOffset, offsets);
    }

    public bool TryReadSpanDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic[]? results, int length, nuint baseAddress, params int[] offsets)
    {
        ThrowHelper.ThrowIfLessThan(length, 0);

        results = new dynamic[length];
        return TryReadSpanDef(definition, results, baseAddress, offsets);
    }

    public bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, uint baseOffset, params int[] offsets)
    {
        return TryReadSpanDef(definition, buffer, MainModule, baseOffset, offsets);
    }

    public bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets)
    {
        if (moduleName is null)
        {
            return false;
        }

        return TryReadSpanDef(definition, buffer, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            return false;
        }

        return TryReadSpanDef(definition, buffer, module.Base + baseOffset, offsets);
    }

    public unsafe bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, nuint baseAddress, params int[] offsets)
    {
        uint size = definition.Size;
        long bytes = size * buffer.Length;

        byte[]? rented = null;
        Span<byte> bBuffer =
            bytes <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPool<byte>.Shared.Rent((int)bytes));

        if (!TryReadSpan(bBuffer, baseAddress, offsets))
        {
            return false;
        }

        fixed (byte* pBuffer = bBuffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = definition.CreateInstance(pBuffer + (i * size));
            }
        }

        ArrayPool<byte>.Shared.ReturnIfNotNull(rented);

        return true;
    }
}
