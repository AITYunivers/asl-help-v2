namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV1Manager
{
    protected virtual nuint GListData(nuint gList)
    {
        return _memory.Read<nuint>(gList + Structs["GList"]["data"]);
    }

    protected virtual nuint GListNext(nuint gList)
    {
        return _memory.Read<nuint>(gList + Structs["GList"]["next"]);
    }

    protected virtual nuint MonoAssemblyImage(nuint assembly)
    {
        return _memory.Read<nuint>(assembly + Structs["MonoAssembly"]["image"]);
    }

    protected virtual nuint MonoClassFields(nuint klass)
    {
        return _memory.Read<nuint>(klass + Structs["MonoClass"]["fields"]);
    }

    protected virtual uint MonoClassFieldCount(nuint klass)
    {
        return _memory.Read<uint>(klass + Structs["MonoClass"]["field.count"]);
    }

    protected virtual nuint MonoClassNextClassCache(nuint klass)
    {
        return _memory.Read<nuint>(klass + Structs["MonoClass"]["next_class_cache"]);
    }
}
