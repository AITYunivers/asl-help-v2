namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV2Manager
{
    protected override uint MonoClassFieldCount(nuint klass)
    {
        MonoTypeKind classKind = MonoClassClassKind(klass);

        if (classKind is MonoTypeKind.DEF or MonoTypeKind.GTD)
        {
            _ = _memory.TryRead(out uint fieldCount, klass + _structs["MonoClassDef"]["field_count"]);
            return fieldCount;
        }
        else if (classKind is MonoTypeKind.GINST)
        {
            nuint genericClass = MonoClassGenericClass(klass);
            return MonoClassFieldCount(genericClass);
        }
        else
        {
            return default;
        }
    }

    protected virtual MonoTypeKind MonoClassClassKind(nuint klass)
    {
        if (!_memory.TryRead(out uint classKind, klass + _structs["MonoClass"]["class_kind"]))
        {
            return default;
        }

        classKind &= _structs["MonoClass"]["class_kind"];

        return (MonoTypeKind)classKind;
    }

    protected virtual nuint MonoClassGenericClass(nuint klass)
    {
        if (!_memory.TryRead(out nuint genericClass, klass + _structs["MonoClass"]["generic_class"]))
        {
            return default;
        }

        if (!_memory.TryRead(out nuint containerClass, genericClass + _structs["MonoGenericClass"]["container_class"]))
        {
            return default;
        }

        return containerClass;
    }

    protected override nuint MonoClassNextClassCache(nuint klass)
    {
        if (!_memory.TryRead(out nuint nextClassCache, klass + _structs["MonoClass"]["next_class_cache"]))
        {
            return default;
        }

        return nextClassCache;
    }
}
