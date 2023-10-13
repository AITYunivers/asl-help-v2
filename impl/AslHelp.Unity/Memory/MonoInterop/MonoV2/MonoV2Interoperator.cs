using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV2Interoperator : MonoV1Interoperator
{
    public MonoV2Interoperator(
        IMonoMemoryManager memory,
        NativeStructMap structs,
        nuint loadedAssemblies,
        nuint defaults)
        : base(memory, structs, loadedAssemblies, defaults) { }
}
