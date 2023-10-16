using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop.Management;

public partial class MonoV1Manager : MonoManager
{
    public MonoV1Manager(
        IMonoMemoryManager memory,
        NativeStructMap structs,
        nuint loadedAssemblies,
        nuint defaults)
        : base(memory, structs, loadedAssemblies, defaults) { }
}
