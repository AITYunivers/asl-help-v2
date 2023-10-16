using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop.Management;

public partial class MonoV2Manager : MonoV1Manager
{
    public MonoV2Manager(
        IMonoMemoryManager memory,
        NativeStructMap structs,
        nuint loadedAssemblies,
        nuint defaults)
        : base(memory, structs, loadedAssemblies, defaults) { }
}
