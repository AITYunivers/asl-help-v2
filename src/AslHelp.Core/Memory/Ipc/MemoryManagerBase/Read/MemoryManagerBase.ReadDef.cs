using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Reflection;

namespace AslHelp.Core.Memory.Ipc;

public partial class MemoryManagerBase
{
    public dynamic ReadDef(ITypeDefinition definition, uint baseOffset, params int[] offsets)
    {
        Module? module = MainModule;
        if (module is null)
        {
            string msg = "[ReadDef] MainModule was null.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ReadDef(definition, module, baseOffset, offsets);
    }

    public dynamic ReadDef(ITypeDefinition definition, string moduleName, uint baseOffset, params int[] offsets)
    {
        Module? module = Modules[moduleName];
        if (module is null)
        {
            string msg = $"[ReadDef] Module '{moduleName}' could not be found.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return ReadDef(definition, module, baseOffset, offsets);
    }

    public dynamic ReadDef(ITypeDefinition definition, Module module, uint baseOffset, params int[] offsets)
    {
        return ReadDef(definition, module.Base + baseOffset, offsets);
    }

    public unsafe dynamic ReadDef(ITypeDefinition definition, nuint baseAddress, params int[] offsets)
    {
        nuint handle = _processHandle;
        uint size = definition.Size;

        byte[]? rented = null;
        Span<byte> buffer =
            size <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPool<byte>.Shared.Rent((int)size));

        ReadSpan(buffer, baseAddress, offsets);

        fixed (byte* pBuffer = buffer)
        {
            return definition.CreateInstance(pBuffer);
        }
    }

    public bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, uint baseOffset, params int[] offsets)
    {
        return TryReadDef(definition, out result, MainModule, baseOffset, offsets);
    }

    public bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, [NotNullWhen(true)] string? moduleName, uint baseOffset, params int[] offsets)
    {
        if (moduleName is null)
        {
            result = default;
            return false;
        }

        return TryReadDef(definition, out result, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, [NotNullWhen(true)] Module? module, uint baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            result = default;
            return false;
        }

        return TryReadDef(definition, out result, module.Base + baseOffset, offsets);
    }

    public unsafe bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, nuint baseAddress, params int[] offsets)
    {
        nuint handle = _processHandle;
        uint size = definition.Size;

        byte[]? rented = null;
        Span<byte> buffer =
            size <= 1024
            ? stackalloc byte[1024]
            : (rented = ArrayPool<byte>.Shared.Rent((int)size));

        if (!TryReadSpan(buffer, baseAddress, offsets))
        {
            result = default;
            return false;
        }

        fixed (byte* pBuffer = buffer)
        {
            result = definition.CreateInstance(pBuffer);
            return true;
        }
    }
}
