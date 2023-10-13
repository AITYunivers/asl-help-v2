using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV1Interoperator : MonoInteroperatorBase
{
    public MonoV1Interoperator(
        IMonoMemoryManager memory,
        NativeStructMap structs,
        nuint loadedAssemblies,
        nuint defaults)
        : base(memory, structs, loadedAssemblies, defaults) { }
}
