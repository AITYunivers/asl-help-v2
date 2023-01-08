using AslHelp.Core.Exceptions;
using AslHelp.Core.Memory.Pointers;
using LiveSplit.ComponentUtil;

public partial class Basic
{
    public uint Tick { get; private set; }

    public Pointer<T> Make<T>(int baseOffset, params int[] offsets) where T : unmanaged
    {
        return Make<T>(MainModule, baseOffset, offsets);
    }

    public Pointer<T> Make<T>(string module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return Make<T>(Modules[module], baseOffset, offsets);
    }

    public Pointer<T> Make<T>(Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        ThrowHelper.ThrowIfNull(module, $"[Make<{typeof(T).Name}>] Module cannot be found.");

        return new(this, module.Base + baseOffset, offsets);
    }

    public Pointer<T> Make<T>(nint baseAddress, params int[] offsets) where T : unmanaged
    {
        ThrowHelper.ThrowIfNull(baseAddress, $"[Make<{typeof(T).Name}>] The base address cannot be 0.");

        return new(this, baseAddress, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return MakeSpan<T>(length, MainModule, baseOffset, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, string module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        return MakeSpan<T>(length, Modules[module], baseOffset, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, Module module, int baseOffset, params int[] offsets) where T : unmanaged
    {
        ThrowHelper.ThrowIfNull(module, $"[MakeSpan<{typeof(T).Name}>] Module cannot be found.");

        return new(this, length, module.Base + baseOffset, offsets);
    }

    public SpanPointer<T> MakeSpan<T>(int length, nint baseAddress, params int[] offsets) where T : unmanaged
    {
        ThrowHelper.ThrowIfNull(baseAddress, $"[MakeSpan<{typeof(T).Name}>] The base address cannot be 0.");

        return new(this, length, baseAddress, offsets);
    }

    public StringPointer MakeString(int baseOffset, params int[] offsets)
    {
        return MakeString(128, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
    }

    public StringPointer MakeString(int length, int baseOffset, params int[] offsets)
    {
        return MakeString(length, ReadStringType.AutoDetect, MainModule, baseOffset, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return MakeString(128, stringType, MainModule, baseOffset, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, int baseOffset, params int[] offsets)
    {
        return MakeString(length, stringType, MainModule, baseOffset, offsets);
    }

    public StringPointer MakeString(string module, int baseOffset, params int[] offsets)
    {
        return MakeString(128, ReadStringType.AutoDetect, Modules[module], baseOffset, offsets);
    }

    public StringPointer MakeString(int length, string module, int baseOffset, params int[] offsets)
    {
        return MakeString(length, ReadStringType.AutoDetect, Modules[module], baseOffset, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        return MakeString(128, stringType, Modules[module], baseOffset, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, string module, int baseOffset, params int[] offsets)
    {
        return MakeString(length, stringType, Modules[module], baseOffset, offsets);
    }

    public StringPointer MakeString(Module module, int baseOffset, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(module, "[MakeString] Module cannot be found.");

        return new(this, 128, ReadStringType.AutoDetect, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, Module module, int baseOffset, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(module, "[MakeString] Module cannot be found.");

        return new(this, length, ReadStringType.AutoDetect, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(module, "[MakeString] Module cannot be found.");

        return new(this, 128, stringType, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, Module module, int baseOffset, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(module, "[MakeString] Module cannot be found.");

        return new(this, length, stringType, module.Base + baseOffset, offsets);
    }

    public StringPointer MakeString(nint baseAddress, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(baseAddress, "[MakeString] The base address cannot be 0.");

        return new(this, 128, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public StringPointer MakeString(int length, nint baseAddress, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(baseAddress, "[MakeString] The base address cannot be 0.");

        return new(this, length, ReadStringType.AutoDetect, baseAddress, offsets);
    }

    public StringPointer MakeString(ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(baseAddress, "[MakeString] The base address cannot be 0.");

        return new(this, 128, stringType, baseAddress, offsets);
    }

    public StringPointer MakeString(int length, ReadStringType stringType, nint baseAddress, params int[] offsets)
    {
        ThrowHelper.ThrowIfNull(baseAddress, "[MakeString] The base address cannot be 0.");

        return new(this, length, stringType, baseAddress, offsets);
    }
}
