using System.Diagnostics;

using AslHelp.Core.IO.Parsing;
using AslHelp.Core.Memory.Ipc;
using AslHelp.Mono.Memory.Ipc;

public partial class Mono : Basic
{
    public void Test()
    {
        var nsm = NativeStructMap.Parse("Mono", "il2cpp", "v29", true);
        Debug.Warn(nsm);
    }

    protected override IMemoryManager InitializeMemory(Process process)
    {
        return new MonoExternalMemoryManager(process, Logger);
    }
}
