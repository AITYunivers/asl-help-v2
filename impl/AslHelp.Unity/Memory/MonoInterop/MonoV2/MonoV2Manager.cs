using AslHelp.Common.Results;
using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoV2Manager : MonoV1Manager
{
    public MonoV2Manager(IMonoMemoryManager memory)
        : base(memory) { }

    protected override Result<NativeStructMap, ParseError> InitializeStructs()
    {
        return NativeStructMap.InitializeFromResource("Unity", "mono", "v2", _memory.Is64Bit);
    }
}
