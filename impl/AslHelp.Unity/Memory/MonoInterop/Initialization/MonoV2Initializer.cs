using AslHelp.Common.Results;
using AslHelp.Core.IO.Parsing;
using AslHelp.Core.Memory.SignatureScanning;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop.Initialization;

public class MonoV2Initializer : MonoV1Initializer
{
    public override Result<NativeStructMap, MonoInitializationError> InitializeStructs(IMonoMemoryManager memory)
    {
        var result = NativeStructMap.InitializeFromResource("Unity", "mono", "v2", memory.Is64Bit);
        if (!result.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: new(MonoInitializationError.StructInitializationFailed, result.Error.Message));
        }

        return new(
            IsSuccess: true,
            Value: result.Value);
    }
}
