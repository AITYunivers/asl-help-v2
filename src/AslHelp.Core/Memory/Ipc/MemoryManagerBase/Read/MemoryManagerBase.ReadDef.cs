﻿using System;
using System.Diagnostics.CodeAnalysis;

using AslHelp.Common.Exceptions;
using AslHelp.Core.Reflection;

namespace AslHelp.Core.Memory.Ipc;

public abstract partial class MemoryManagerBase
{
    public dynamic? ReadDef(ITypeDefinition definition, int baseOffset, params int[] offsets)
    {
        return ReadDef(definition, MainModule, baseOffset, offsets);
    }

    public dynamic? ReadDef(ITypeDefinition definition, string moduleName, int baseOffset, params int[] offsets)
    {
        return ReadDef(definition, Modules[moduleName], baseOffset, offsets);
    }

    public dynamic? ReadDef(ITypeDefinition definition, Module? module, int baseOffset, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(module, nameof(module));

        return ReadDef(definition, module.Base + baseOffset, offsets);
    }

    public abstract dynamic? ReadDef(ITypeDefinition definition, nint baseAddress, params int[] offsets);

    public bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, int baseOffset, params int[] offsets)
    {
        return TryReadDef(definition, out result, MainModule, baseOffset, offsets);
    }

    public bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, string moduleName, int baseOffset, params int[] offsets)
    {
        return TryReadDef(definition, out result, Modules[moduleName], baseOffset, offsets);
    }

    public bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, Module? module, int baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            Debug.Warn("[TryReadCustom] Module could not be found.");

            result = default;
            return false;
        }

        return TryReadDef(definition, out result, module.Base + baseOffset, offsets);
    }

    public unsafe bool TryReadDef(ITypeDefinition definition, [NotNullWhen(true)] out dynamic? result, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            result = definition.Default;
            return false;
        }

        int size = definition.Size;
        Span<byte> buffer = stackalloc byte[size];

        if (!TryReadSpan(buffer, deref))
        {
            result = definition.Default;
            return false;
        }

        fixed (byte* pBuffer = buffer)
        {
            result = definition.CreateInstance(pBuffer);
            return true;
        }
    }
}
