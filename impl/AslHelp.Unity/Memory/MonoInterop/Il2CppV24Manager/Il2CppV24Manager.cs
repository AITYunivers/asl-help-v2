using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class Il2CppV24Manager : MonoManagerBase
{
    public Il2CppV24Manager(IMonoMemoryManager memory)
        : base(memory) { }
}
