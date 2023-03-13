using AslHelp.Core.Memory.IO;

namespace AslHelp.Core.Memory.Pointers.Commands;

internal sealed class MakeSpanPointerCommand<T>
    : MakePointerCommandBase
    where T : unmanaged
{
    private readonly int _length;

    public MakeSpanPointerCommand(int length, int baseOffset, int[] offsets)
        : base(baseOffset, offsets)
    {
        _length = length;
    }

    public MakeSpanPointerCommand(int length, string moduleName, int baseOffset, params int[] offsets)
        : base(moduleName, baseOffset, offsets)
    {
        _length = length;
    }

    public MakeSpanPointerCommand(int length, Module module, int baseOffset, params int[] offsets)
        : base(module, baseOffset, offsets)
    {
        _length = length;
    }

    public MakeSpanPointerCommand(int length, nint baseAddress, int[] offsets)
        : base(baseAddress, offsets)
    {
        _length = length;
    }

    public override bool TryExecute(IMemoryManager manager, out IPointer pointer)
    {
        if (!TryGetBaseAddress(manager, out nint baseAddress))
        {
            pointer = default;
            return false;
        }

        pointer = new SpanPointer<T>(manager, _length, baseAddress, _offsets);
        return true;
    }
}
