using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop.Management;

public abstract partial class Il2CppManager : MonoManager
{
    protected readonly nuint _typeInfoDefinitions;

    protected Il2CppManager(
        IMonoMemoryManager memory,
        NativeStructMap structs,
        nuint loadedAssemblies,
        nuint typeInfoDefinitions,
        nuint defaults)
        : base(memory, structs, loadedAssemblies, defaults)
    {
        _typeInfoDefinitions = typeInfoDefinitions;
    }
}
