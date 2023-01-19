using AslHelp.Core.Reflection;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryIOBase
{
    public dynamic ReadDef(ITypeDefinition definition, int baseOffset, params int[] offsets)
    {
        TryReadDef(definition, out dynamic result, _manager.MainModule, baseOffset, offsets);
        return result;
    }

    public dynamic ReadDef(ITypeDefinition definition, string module, int baseOffset, params int[] offsets)
    {
        TryReadDef(definition, out dynamic result, _manager.Modules[module], baseOffset, offsets);
        return result;
    }

    public dynamic ReadDef(ITypeDefinition definition, Module module, int baseOffset, params int[] offsets)
    {
        TryReadDef(definition, out dynamic result, module, baseOffset, offsets);
        return result;
    }

    public dynamic ReadDef(ITypeDefinition definition, nint baseAddress, params int[] offsets)
    {
        TryReadDef(definition, out dynamic result, baseAddress, offsets);
        return result;
    }

    public bool TryReadDef(ITypeDefinition definition, out dynamic result, int baseOffset, params int[] offsets)
    {
        return TryReadDef(definition, out result, _manager.MainModule, baseOffset, offsets);
    }

    public bool TryReadDef(ITypeDefinition definition, out dynamic result, string module, int baseOffset, params int[] offsets)
    {
        return TryReadDef(definition, out result, _manager.Modules[module], baseOffset, offsets);
    }

    public bool TryReadDef(ITypeDefinition definition, out dynamic result, Module module, int baseOffset, params int[] offsets)
    {
        if (module is null)
        {
            Debug.Warn($"[ReadCustom] Module could not be found.");

            result = default;
            return false;
        }

        return TryReadDef(definition, out result, module.Base + baseOffset, offsets);
    }

    public unsafe bool TryReadDef(ITypeDefinition definition, out dynamic result, nint baseAddress, params int[] offsets)
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
            result = definition.Convert(pBuffer);
            return true;
        }
    }
}
