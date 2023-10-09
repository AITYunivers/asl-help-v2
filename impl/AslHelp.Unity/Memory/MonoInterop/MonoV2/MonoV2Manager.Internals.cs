namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV2Manager
{
    protected override uint MonoClassFieldCount(nuint klass)
    {
        MonoTypeKind classKind = MonoClassClassKind(klass);

        if (classKind is MonoTypeKind.DEF or MonoTypeKind.GTD)
        {
            _ = _memory.TryRead(out uint fieldCount, klass + Structs["MonoClassDef"]["field_count"]);
            return fieldCount;
        }
        else if (classKind is MonoTypeKind.GINST)
        {
            nuint genericClass = MonoClassGenericClass(klass);
            return MonoClassFieldCount(genericClass);
        }
        else
        {
            return 0;
        }
    }

    protected virtual MonoTypeKind MonoClassClassKind(nuint klass)
    {
        uint classKind = _memory.Read<uint>(klass + Structs["MonoClass"]["class_kind"]);
        classKind &= Structs["MonoClass"]["class_kind"];

        return (MonoTypeKind)classKind;
    }

    protected virtual nuint MonoClassGenericClass(nuint klass)
    {
        nuint genericClass = _memory.Read<nuint>(klass + Structs["MonoClassGenericInst"]["generic_class"]);
        return _memory.Read<nuint>(genericClass + Structs["MonoGenericClass"]["container_class"]);
    }

    protected override nuint MonoClassNextClassCache(nuint klass)
    {
        return _memory.Read<nuint>(klass + Structs["MonoClassDef"]["next_class_cache"]);
    }
}
