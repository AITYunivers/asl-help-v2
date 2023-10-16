using System.Text;

using AslHelp.Unity.Memory.MonoInterop;
using AslHelp.Unity.Memory.MonoInterop.Management;

namespace AslHelp.Unity.Memory;

public class MonoType(
    nuint address,
    MonoManager mono)
{
    private readonly MonoManager _mono = mono;

    public nuint Address { get; } = address;

    private MonoClass? _class = null;
    public MonoClass Class
    {
        get
        {
            if (_class is not null)
            {
                return _class;
            }

            nuint klass = _mono.GetTypeClass(Address);
            if (!_mono.ClassCache.TryGetValue(klass, out MonoClass? monoClass))
            {
                monoClass = new(klass, _mono);
                _mono.ClassCache[klass] = monoClass;
            }

            _class = monoClass;
            return _class;
        }
    }

    private MonoFieldAttribute? _attributes;
    public MonoFieldAttribute Attributes => _attributes ??= _mono.GetTypeAttributes(Address);

    public override string ToString()
    {
        StringBuilder sb = new();

        MonoFieldAttribute attr = Attributes;

        sb.Append((attr & MonoFieldAttribute.FIELD_ACCESS) switch
        {
            MonoFieldAttribute.PRIVATE => "private ",
            MonoFieldAttribute.FAM_AND_ASSEM => "private protected ",
            MonoFieldAttribute.ASSEMBLY => "internal ",
            MonoFieldAttribute.FAMILY => "protected ",
            MonoFieldAttribute.FAM_OR_ASSEM => "protected internal ",
            MonoFieldAttribute.PUBLIC => "public ",
            _ => "<unknown>"
        });

        if (hasFlag(MonoFieldAttribute.LITERAL))
        {
            sb.Append("const ");
        }
        else if (hasFlag(MonoFieldAttribute.STATIC))
        {
            sb.Append("static ");
        }

        if (hasFlag(MonoFieldAttribute.INIT_ONLY))
        {
            sb.Append("readonly ");
        }

        sb.Append(Class);

        return sb.ToString();

        bool hasFlag(MonoFieldAttribute flag)
        {
            return (attr & flag) == flag;
        }
    }
}
