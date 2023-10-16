using AslHelp.Core.Collections;
using AslHelp.Unity.Memory.MonoInterop.Management;

namespace AslHelp.Unity.Memory;

public class MonoClass(
    nuint address,
    MonoManager mono)
{
    private readonly MonoManager _mono = mono;

    public nuint Address { get; } = address;

    private string? _name;
    public string Name => _name ??= _mono.GetClassName(Address);

    private string? _namespace;
    public string Namespace => _namespace ??= _mono.GetClassNamespace(Address);

    private MonoClass? _parent;
    public MonoClass Parent => _parent ??= new(_mono.GetClassParent(Address), _mono);

    private nuint? _staticDataChunk;
    public nuint StaticDataChunk => _staticDataChunk ??= _mono.GetClassStaticDataChunk(Address);

    public LazyDictionary<string, MonoField> Fields { get; } = new MonoFieldCache(address, mono);

    public override string ToString()
    {
        if (string.IsNullOrEmpty(Namespace))
        {
            return Name;
        }
        else
        {
            return $"{Namespace}.{Name}";
        }
    }
}
