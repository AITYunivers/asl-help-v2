namespace AslHelp.Unity.Memory.MonoInterop.Management;

public partial class MonoV1Manager
{
    protected virtual nuint GListData(nuint gList)
    {
        return _memory.Read<nuint>(gList + _structs["GList"]["data"]);
    }

    protected virtual nuint GListNext(nuint gList)
    {
        return _memory.Read<nuint>(gList + _structs["GList"]["next"]);
    }

    protected virtual nuint MonoAssemblyImage(nuint assembly)
    {
        return _memory.Read<nuint>(assembly + _structs["MonoAssembly"]["image"]);
    }

    protected virtual nuint MonoClassFields(nuint klass)
    {
        return _memory.Read<nuint>(klass + _structs["MonoClass"]["fields"]);
    }

    protected virtual uint MonoClassFieldCount(nuint klass)
    {
        return _memory.Read<uint>(klass + _structs["MonoClass"]["field.count"]);
    }

    protected virtual nuint MonoClassNextClassCache(nuint klass)
    {
        return _memory.Read<nuint>(klass + _structs["MonoClass"]["next_class_cache"]);
    }

    protected virtual nuint MonoClassVTable(nuint klass)
    {
        nuint runtime_info = _memory.Read<nuint>(klass + _structs["MonoClass"]["runtime_info"]);
        return _memory.Read<nuint>(runtime_info + _structs["MonoClassRuntimeInfo"]["domain_vtables"]);
    }

    public virtual nuint MonoGenericClassClass(nuint genericClass)
    {
        return _memory.Read<nuint>(genericClass + _structs["MonoGenericClass"]["container_class"]);
    }

    public virtual nuint MonoTypeData(nuint type)
    {
        return _memory.Read<nuint>(type + _structs["MonoType"]["data"]);
    }

    public virtual nuint MonoArrayTypeClass(nuint arrayType)
    {
        return _memory.Read<nuint>(arrayType + _structs["MonoArrayType"]["eklass"]);
    }
}
