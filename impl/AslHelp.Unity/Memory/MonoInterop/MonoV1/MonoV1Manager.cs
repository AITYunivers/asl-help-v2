using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV1Manager : MonoManagerBase
{
    public MonoV1Manager(IMonoMemoryManager memory)
        : base(memory) { }
}
