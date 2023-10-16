namespace AslHelp.Unity.Memory.MonoInterop.Management;

public partial class MonoV2Manager
{
    public override nuint GetClassStaticDataChunk(nuint klass)
    {
        uint vtableSize = (uint)_memory.Read<int>(klass + _structs["MonoClass"]["vtable_size"]);
        return _memory.Read<nuint>(MonoClassVTable(klass) + _structs["MonoVTable"]["vtable"] + (_memory.PointerSize * vtableSize));
    }
}
