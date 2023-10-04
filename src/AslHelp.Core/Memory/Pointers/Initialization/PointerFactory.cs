using System.Collections.Generic;

using AslHelp.Common.Memory;
using AslHelp.Core.Memory.Ipc;

using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers.Initialization;

public class PointerFactory : Dictionary<string, IPointer>, IPointerFactory
{
    protected readonly IMemoryManager _memory;

    public PointerFactory(IMemoryManager memory)
    {
        _memory = memory;
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
        return new(_memory, baseAddress, offsets);
    }

    public Pointer<T> Make<T>(uint baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_memory, _memory.MainModule.Base + baseOffset, offsets);
    }

    public Pointer<T> Make<T>(string moduleName, uint baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_memory, _memory.Modules[moduleName].Base + baseOffset, offsets);
    }

    public Pointer<T> Make<T>(Module module, uint baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_memory, module.Base + baseOffset, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, nuint baseAddress, params int[] offsets)
        where T : unmanaged
    {
        return new(_memory, length, baseAddress, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, uint baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_memory, length, _memory.MainModule.Base + baseOffset, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, string moduleName, uint baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_memory, length, _memory.Modules[moduleName].Base + baseOffset, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, Module module, uint baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_memory, length, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(nuint baseAddress, params int[] offsets)
    {
        return new(_memory, IOR.DefaultStringReadLength, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public StringPointer MakeString(uint baseOffset, params int[] offsets)
    {
        return new(_memory, IOR.DefaultStringReadLength, ReadStringType.AutoDetect, _memory.MainModule.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(string moduleName, uint baseOffset, params int[] offsets)
    {
        return new(_memory, IOR.DefaultStringReadLength, ReadStringType.AutoDetect, _memory.Modules[moduleName].Base + baseOffset, offsets);
    }

    public StringPointer MakeString(Module module, uint baseOffset, params int[] offsets)
    {
        return new(_memory, IOR.DefaultStringReadLength, ReadStringType.AutoDetect, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, nuint baseAddress, params int[] offsets)
    {
        return new(_memory, length, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public StringPointer MakeString(int length, uint baseOffset, params int[] offsets)
    {
        return new(_memory, length, ReadStringType.AutoDetect, _memory.MainModule.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, string moduleName, uint baseOffset, params int[] offsets)
    {
        return new(_memory, length, ReadStringType.AutoDetect, _memory.Modules[moduleName].Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, Module module, uint baseOffset, params int[] offsets)
    {
        return new(_memory, length, ReadStringType.AutoDetect, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, nuint baseAddress, params int[] offsets)
    {
        return new(_memory, IOR.DefaultStringReadLength, stringType, baseAddress, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, uint baseOffset, params int[] offsets)
    {
        return new(_memory, IOR.DefaultStringReadLength, stringType, _memory.MainModule.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets)
    {
        return new(_memory, IOR.DefaultStringReadLength, stringType, _memory.Modules[moduleName].Base + baseOffset, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, Module module, uint baseOffset, params int[] offsets)
    {
        return new(_memory, IOR.DefaultStringReadLength, stringType, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, nuint baseAddress, params int[] offsets)
    {
        return new(_memory, length, stringType, baseAddress, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, uint baseOffset, params int[] offsets)
    {
        return new(_memory, length, stringType, _memory.MainModule.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets)
    {
        return new(_memory, length, stringType, _memory.Modules[moduleName].Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, Module module, uint baseOffset, params int[] offsets)
    {
        return new(_memory, length, stringType, module.Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(nuint baseAddress, params int[] offsets)
    {
        return new(_memory, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public SizedStringPointer MakeSizedString(uint baseOffset, params int[] offsets)
    {
        return new(_memory, ReadStringType.AutoDetect, _memory.MainModule.Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(string moduleName, uint baseOffset, params int[] offsets)
    {
        return new(_memory, ReadStringType.AutoDetect, _memory.Modules[moduleName].Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(Module module, uint baseOffset, params int[] offsets)
    {
        return new(_memory, ReadStringType.AutoDetect, module.Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, nuint baseAddress, params int[] offsets)
    {
        return new(_memory, stringType, baseAddress, offsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, uint baseOffset, params int[] offsets)
    {
        return new(_memory, stringType, _memory.MainModule.Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, string moduleName, uint baseOffset, params int[] offsets)
    {
        return new(_memory, stringType, _memory.Modules[moduleName].Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, Module module, uint baseOffset, params int[] offsets)
    {
        return new(_memory, stringType, module.Base + baseOffset, offsets);
    }
}

