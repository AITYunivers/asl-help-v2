using AslHelp.Unity.Memory.MonoInterop;

namespace AslHelp.Unity.Memory;

public class MonoField(
    nuint address,
    IMonoInteroperator mono)
{
    private readonly IMonoInteroperator _mono = mono;

    public nuint Address { get; } = address;

    private string? _name;
    public string Name
    {
        get
        {
            if (_name is not null)
            {
                return _name;
            }

            string name = _mono.GetFieldName(Address);

            _name =
                name is ['<', .. string propertyName, '>', 'k', '_', '_', 'B', 'a', 'c', 'k', 'i', 'n', 'g', 'F', 'i', 'e', 'l', 'd']
                ? propertyName
                : name;

            return _name;
        }
    }

    private int? _offset;
    public int Offset => _offset ??= _mono.GetFieldOffset(Address);

    private MonoType? _type;
    public MonoType Type => _type ??= new(_mono.GetFieldType(Address), _mono);
}
