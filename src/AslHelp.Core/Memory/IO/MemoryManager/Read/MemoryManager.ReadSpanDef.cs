using AslHelp.Core.Reflection;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryManagerBase
{
    public dynamic[] ReadSpanDef(ITypeDefinition definition, int length, int baseOffset, params int[] offsets)
    {
        dynamic[] results = new dynamic[length];
        _ = TryReadSpanDef(definition, results, MainModule, baseOffset, offsets);

        return results;
    }

    public dynamic[] ReadSpanDef(ITypeDefinition definition, int length, string module, int baseOffset, params int[] offsets)
    {
        dynamic[] results = new dynamic[length];
        _ = TryReadSpanDef(definition, results, Modules[module], baseOffset, offsets);

        return results;
    }

    public dynamic[] ReadSpanDef(ITypeDefinition definition, int length, Module module, int baseOffset, params int[] offsets)
    {
        dynamic[] results = new dynamic[length];
        _ = TryReadSpanDef(definition, results, module, baseOffset, offsets);

        return results;
    }

    public dynamic[] ReadSpanDef(ITypeDefinition definition, int length, nint baseAddress, params int[] offsets)
    {
        dynamic[] results = new dynamic[length];
        _ = TryReadSpanDef(definition, results, baseAddress, offsets);

        return results;
    }

    public bool TryReadSpanDef(ITypeDefinition definition, out dynamic[] results, int length, int baseOffset, params int[] offsets)
    {
        results = new dynamic[length];
        return TryReadSpanDef(definition, results, MainModule, baseOffset, offsets);
    }

    public bool TryReadSpanDef(ITypeDefinition definition, out dynamic[] results, int length, string module, int baseOffset, params int[] offsets)
    {
        results = new dynamic[length];
        return TryReadSpanDef(definition, results, Modules[module], baseOffset, offsets);
    }

    public bool TryReadSpanDef(ITypeDefinition definition, out dynamic[] results, int length, Module module, int baseOffset, params int[] offsets)
    {
        results = new dynamic[length];
        return TryReadSpanDef(definition, results, module, baseOffset, offsets);
    }

    public bool TryReadSpanDef(ITypeDefinition definition, out dynamic[] results, int length, nint baseAddress, params int[] offsets)
    {
        results = new dynamic[length];
        return TryReadSpanDef(definition, results, baseAddress, offsets);
    }

    public bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, int baseOffset, params int[] offsets)
    {
        return TryReadSpanDef(definition, buffer, MainModule, baseOffset, offsets);
    }

    public bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, string module, int baseOffset, params int[] offsets)
    {
        return TryReadSpanDef(definition, buffer, Modules[module], baseOffset, offsets);
    }

    public bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, Module module, int baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            Debug.Warn($"[ReadSpanDef] Module could not be found.");

            return false;
        }

        return TryReadSpanDef(definition, buffer, module.Base + baseOffset, offsets);
    }

    public unsafe bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, nint baseAddress, params int[] offsets)
    {
        if (!TryDeref(out nint deref, baseAddress, offsets))
        {
            buffer.Fill(definition.Default);
            return false;
        }

        int size = definition.Size;
        Span<byte> bBuffer = stackalloc byte[size];

        if (!TryReadSpan<byte>(bBuffer, deref))
        {
            buffer.Fill(definition.Default);
            return false;
        }

        for (int i = 0; i < buffer.Length; i++)
        {
            fixed (byte* pBuffer = bBuffer.Slice(size * i, size))
            {
                buffer[i] = definition.CreateInstance(pBuffer);
            }
        }

        return true;
    }
}
