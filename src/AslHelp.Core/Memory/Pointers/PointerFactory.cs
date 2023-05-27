using AslHelp.Common.Resources;
using AslHelp.Core.Memory.IO;
using LiveSplit.ComponentUtil;

namespace AslHelp.Core.Memory.Pointers;

public class PointerFactory
{
    private readonly IMemoryManager _manager;

    public PointerFactory(IMemoryManager manager)
    {
        _manager = manager;
    }

    public Pointer<T> Make<T>(nint baseAddress, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, baseAddress, offsets);
    }

    public Pointer<T> Make<T>(int baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, _manager.MainModule.Base + baseOffset, offsets);
    }

    public Pointer<T> Make<T>(string moduleName, int baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public Pointer<T> Make<T>(Module module, int baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, module.Base + baseOffset, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, nint baseAddress, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, length, baseAddress, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, int baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, length, _manager.MainModule.Base + baseOffset, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, string moduleName, int baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, length, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, Module module, int baseOffset, params int[] offsets)
        where T : unmanaged
    {
        return new(_manager, length, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(nint baseAddress, params int[] offsets)
    {
        return new(_manager, AHR.MaxStringReadLength, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public StringPointer MakeString(int baseOffset, params int[] offsets)
    {
        return new(_manager, AHR.MaxStringReadLength, ReadStringType.AutoDetect, _manager.MainModule.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(string moduleName, int baseOffset, params int[] offsets)
    {
        return new(_manager, AHR.MaxStringReadLength, ReadStringType.AutoDetect, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public StringPointer MakeString(Module module, int baseOffset, params int[] offsets)
    {
        return new(_manager, AHR.MaxStringReadLength, ReadStringType.AutoDetect, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, nint baseAddress, params int[] offsets)
    {
        return new(_manager, length, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public StringPointer MakeString(int length, int baseOffset, params int[] offsets)
    {
        return new(_manager, length, ReadStringType.AutoDetect, _manager.MainModule.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, string moduleName, int baseOffset, params int[] offsets)
    {
        return new(_manager, length, ReadStringType.AutoDetect, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, Module module, int baseOffset, params int[] offsets)
    {
        return new(_manager, length, ReadStringType.AutoDetect, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        return new(_manager, AHR.MaxStringReadLength, stringType, baseAddress, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return new(_manager, AHR.MaxStringReadLength, stringType, _manager.MainModule.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        return new(_manager, AHR.MaxStringReadLength, stringType, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        return new(_manager, AHR.MaxStringReadLength, stringType, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        return new(_manager, length, stringType, baseAddress, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return new(_manager, length, stringType, _manager.MainModule.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        return new(_manager, length, stringType, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        return new(_manager, length, stringType, module.Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(nint baseAddress, params int[] offsets)
    {
        return new(_manager, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public SizedStringPointer MakeSizedString(int baseOffset, params int[] offsets)
    {
        return new(_manager, ReadStringType.AutoDetect, _manager.MainModule.Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(string moduleName, int baseOffset, params int[] offsets)
    {
        return new(_manager, ReadStringType.AutoDetect, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(Module module, int baseOffset, params int[] offsets)
    {
        return new(_manager, ReadStringType.AutoDetect, module.Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        return new(_manager, stringType, baseAddress, offsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return new(_manager, stringType, _manager.MainModule.Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, string moduleName, int baseOffset, params int[] offsets)
    {
        return new(_manager, stringType, _manager.Modules[moduleName].Base + baseOffset, offsets);
    }

    public SizedStringPointer MakeSizedString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        return new(_manager, stringType, module.Base + baseOffset, offsets);
    }
}
