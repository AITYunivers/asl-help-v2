
namespace AslHelp.Unity.Memory.MonoInterop;

public partial class Il2CppV24Manager
{
    protected virtual nuint Il2CppAssemblyImage(nuint assembly)
    {
        return _memory.Read<nuint>(assembly + _structs["Il2CppAssembly"]["image"]);
    }

    protected virtual int Il2CppImageTypeStart(nuint image)
    {
        return _memory.Read<int>(image + _structs["Il2CppImage"]["typeStart"]);
    }

    protected virtual uint Il2CppImageTypeCount(nuint image)
    {
        return _memory.Read<uint>(image + _structs["Il2CppImage"]["typeCount"]);
    }

    protected virtual nuint Il2CppClassFields(nuint klass)
    {
        return _memory.Read<nuint>(klass + _structs["Il2CppClass"]["fields"]);
    }

    protected virtual ushort Il2CppClassFieldCount(nuint klass)
    {
        return _memory.Read<ushort>(klass + _structs["Il2CppClass"]["field_count"]);
    }
}
