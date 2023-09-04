using System.Collections.Generic;

using AslHelp.Common.Memory;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Core.Reflection;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Initialization;

public class PointerFactory : Dictionary<string, IPointer>, IPointerFactory
{
    private readonly IMemoryManager _manager;

    private PointerFactory(IMemoryManager manager)
    {
        _manager = manager;
    }

    public static PointerFactory Create(IMemoryManager manager)
    {
        return new(manager);
    }

    public void MapTo(IDictionary<string, object> destination)
    {
        foreach (var entry in this)
        {
            destination[entry.Key] = entry.Value;
        }
    }

    public Pointer<T> Make<T>(nuint baseAddress, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, baseAddress, offsets);
    }

    public Pointer<T> Make<T>(uint baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, _manager.MainModule.Base + baseOffset, offsets);
    }

    public Pointer<T> Make<T>(string moduleName, uint baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public Pointer<T> Make<T>(Module module, uint baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, module.Base + baseOffset, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, nuint baseAddress, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, length, baseAddress, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, uint baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, length, _manager.MainModule.Base + baseOffset, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, string moduleName, uint baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, length, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, Module module, uint baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, length, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(nuint baseAddress, params int[] offsets)
    {
        return new(_manager, IOR.DefaultStringReadLength, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public StringPointer MakeString(uint baseOffset, params int[] offsets)
    {
        return new(_manager, IOR.DefaultStringReadLength, ReadStringType.AutoDetect, _manager.MainModule.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(string moduleName, uint baseOffset, params int[] offsets)
    {
        return new(_manager, IOR.DefaultStringReadLength, ReadStringType.AutoDetect, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public StringPointer MakeString(Module module, uint baseOffset, params int[] offsets)
    {
        return new(_manager, IOR.DefaultStringReadLength, ReadStringType.AutoDetect, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, nuint baseAddress, params int[] offsets)
    {
        return new(_manager, length, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public StringPointer MakeString(int length, uint baseOffset, params int[] offsets)
    {
        return new(_manager, length, ReadStringType.AutoDetect, _manager.MainModule.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, string moduleName, uint baseOffset, params int[] offsets)
    {
        return new(_manager, length, ReadStringType.AutoDetect, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, Module module, uint baseOffset, params int[] offsets)
    {
        return new(_manager, length, ReadStringType.AutoDetect, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, nuint baseAddress, params int[] offsets)
    {
        return new(_manager, IOR.DefaultStringReadLength, stringType, baseAddress, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, uint baseOffset, params int[] offsets)
    {
        return new(_manager, IOR.DefaultStringReadLength, stringType, _manager.MainModule.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets)
    {
        return new(_manager, IOR.DefaultStringReadLength, stringType, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, Module module, uint baseOffset, params int[] offsets)
    {
        return new(_manager, IOR.DefaultStringReadLength, stringType, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, nuint baseAddress, params int[] offsets)
    {
        return new(_manager, length, stringType, baseAddress, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, uint baseOffset, params int[] offsets)
    {
        return new(_manager, length, stringType, _manager.MainModule.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets)
    {
        return new(_manager, length, stringType, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, Module module, uint baseOffset, params int[] offsets)
    {
        return new(_manager, length, stringType, module.Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(nuint baseAddress, params int[] offsets)
    {
        return new(_manager, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public SizedStringPointer MakeSizedString(uint baseOffset, params int[] offsets)
    {
        return new(_manager, ReadStringType.AutoDetect, _manager.MainModule.Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(string moduleName, uint baseOffset, params int[] offsets)
    {
        return new(_manager, ReadStringType.AutoDetect, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(Module module, uint baseOffset, params int[] offsets)
    {
        return new(_manager, ReadStringType.AutoDetect, module.Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, nuint baseAddress, params int[] offsets)
    {
        return new(_manager, stringType, baseAddress, offsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, uint baseOffset, params int[] offsets)
    {
        return new(_manager, stringType, _manager.MainModule.Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets)
    {
        return new(_manager, stringType, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, Module module, uint baseOffset, params int[] offsets)
    {
        return new(_manager, stringType, module.Base + baseOffset, offsets);
    }

    public TypeDefinitionPointer MakeDef(ITypeDefinition definition, uint baseOffset, params int[] offsets)
    {
        return new(_manager, definition, _manager.MainModule.Base + baseOffset, offsets);
    }

    public TypeDefinitionPointer MakeDef(ITypeDefinition definition, string moduleName, uint baseOffset, params int[] offsets)
    {
        return new(_manager, definition, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public TypeDefinitionPointer MakeDef(ITypeDefinition definition, Module module, uint baseOffset, params int[] offsets)
    {
        return new(_manager, definition, module.Base + baseOffset, offsets);
    }

    public TypeDefinitionPointer MakeDef(ITypeDefinition definition, nuint baseAddress, params int[] offsets)
    {
        return new(_manager, definition, baseAddress, offsets);
    }
}

