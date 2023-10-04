using AslHelp.Unity.Memory.MonoInterop;

namespace AslHelp.Unity;

public class MonoField(nuint address, IMonoManager mono)
{
    private readonly IMonoManager _mono = mono;

    public nuint Address { get; } = address;

    private string? _name;
    public string Name => _name ??= _mono.GetFieldName(Address);

    private int? _offset;
    public int Offset => _offset ??= _mono.GetFieldOffset(Address);
}
