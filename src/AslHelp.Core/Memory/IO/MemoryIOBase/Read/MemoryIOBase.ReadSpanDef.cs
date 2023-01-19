using AslHelp.Core.Reflection;

namespace AslHelp.Core.Memory.IO;

public abstract partial class MemoryIOBase
{
    public dynamic[] ReadSpanDef(ITypeDefinition definition, int length, int baseOffset, params int[] offsets)
    {
        dynamic[] results = new dynamic[length];
        TryReadSpanDef(definition, results, _manager.MainModule, baseOffset, offsets);

        return results;
    }

    public dynamic[] ReadSpanDef(ITypeDefinition definition, int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public dynamic[] ReadSpanDef(ITypeDefinition definition, int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public dynamic[] ReadSpanDef(ITypeDefinition definition, int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadSpanDef(ITypeDefinition definition, out dynamic[] results, int length, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadSpanDef(ITypeDefinition definition, out dynamic[] results, int length, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadSpanDef(ITypeDefinition definition, out dynamic[] results, int length, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadSpanDef(ITypeDefinition definition, out dynamic[] results, int length, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, string module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, Module module, int baseOffset, params int[] offsets)
    {
        throw new NotImplementedException();
    }

    public bool TryReadSpanDef(ITypeDefinition definition, Span<dynamic> buffer, nint baseAddress, params int[] offsets)
    {
        throw new NotImplementedException();
    }
}
