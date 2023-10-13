using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class Il2CppV24Interoperator : Il2CppInteroperatorBase
{
    public Il2CppV24Interoperator(
        IMonoMemoryManager memory,
        NativeStructMap structs,
        nuint loadedAssemblies,
        nuint typeInfoDefinitions,
        nuint defaults)
        : base(memory, structs, loadedAssemblies, typeInfoDefinitions, defaults) { }
}
