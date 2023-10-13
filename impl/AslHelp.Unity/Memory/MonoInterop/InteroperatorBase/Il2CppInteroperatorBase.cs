using AslHelp.Common.Results;
using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;
using AslHelp.Unity.Memory.MonoInterop.Initialization;

namespace AslHelp.Unity.Memory.MonoInterop;

public abstract partial class Il2CppInteroperatorBase : MonoInteroperatorBase
{
    protected readonly nuint _typeInfoDefinitions;

    protected Il2CppInteroperatorBase(
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
