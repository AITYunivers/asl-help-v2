using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop.Management;

public partial class Il2CppV24Manager : Il2CppManager
{
    public Il2CppV24Manager(
        IMonoMemoryManager memory,
        NativeStructMap structs,
        nuint loadedAssemblies,
        nuint typeInfoDefinitions,
        nuint defaults)
        : base(memory, structs, loadedAssemblies, typeInfoDefinitions, defaults) { }
}
