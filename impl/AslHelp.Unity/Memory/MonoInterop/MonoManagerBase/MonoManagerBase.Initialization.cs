using AslHelp.Common.Results;
using AslHelp.Core.IO.Parsing;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop;

public partial class MonoManagerBase
{
    protected abstract Result<NativeStructMap, ParseError> InitializeStructs();
    protected abstract Result<nuint, MonoInitializationError> FindLoadedAssemblies();

    public static Result<IMonoManager, MonoInitializationError> InitializeMono(IMonoMemoryManager memory)
    {
        MonoManagerBase mono;
        if (memory.MonoModule.Name == "mono.dll")
        {
            mono = new MonoV1Manager(memory);
        }
        else if (memory.MonoModule.Name == "mono-2.0-bdwgc.dll")
        {
            mono = new MonoV2Manager(memory);
        }
        else
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.InvalidMonoModule);
        }

        return Initialize(mono);
    }

    public static Result<IMonoManager, MonoInitializationError> InitializeIl2Cpp(IMonoMemoryManager memory, int il2CppVersion)
    {
        if (memory.MonoModule.Name != "GameAssembly.dll")
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.InvalidMonoModule);
        }

        MonoManagerBase mono;
        switch (il2CppVersion)
        {
            case 24:
            {
                mono = new Il2CppV24Manager(memory);
                break;
            }
            default:
            {
                return new(
                    IsSuccess: false,
                    Error: MonoInitializationError.UnsupportedIl2CppVersion);
            }
        }

        return Initialize(mono);
    }

    private static Result<IMonoManager, MonoInitializationError> Initialize(MonoManagerBase mono)
    {
        var structsResult = mono.InitializeStructs();
        if (!structsResult.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.StructInitializationFailed);
        }

        var loadedAssembliesResult = mono.FindLoadedAssemblies();
        if (!loadedAssembliesResult.IsSuccess)
        {
            return new(
                IsSuccess: false,
                Error: loadedAssembliesResult.Error);
        }

        mono.Structs = structsResult.Value;
        mono.LoadedAssemblies = loadedAssembliesResult.Value;

        return new(
            IsSuccess: true,
            Value: mono);
    }
}
