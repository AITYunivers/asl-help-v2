using AslHelp.Common.Results;
using AslHelp.Core.IO.Parsing;
using AslHelp.Core.Memory.SignatureScanning;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop.Initialization;

public class MonoV1Initializer : IMonoInitializer
{
    public virtual Result<NativeStructMap, MonoInitializationError> InitializeStructs(IMonoMemoryManager memory)
    {
        var result = NativeStructMap.InitializeFromResource("Unity", "mono", "v1", memory.Is64Bit);
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

    public virtual Result<nuint, MonoInitializationError> FindLoadedAssemblies(IMonoMemoryManager memory)
    {
        if (!memory.MonoModule.Symbols.TryGetValue("mono_assembly_foreach", out var symMonoAssemblyForeach)
            || symMonoAssemblyForeach.Address == 0)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.LoadedAssembliesNotResolved);
        }

        Signature signature =
            memory.Is64Bit
            ? new(3, "48 8B 0D")
            : new(2, "FF 35");

        nuint loadedAssembliesRelative = memory.Scan(signature, symMonoAssemblyForeach.Address, 0x100);
        if (!memory.TryReadRelative(loadedAssembliesRelative, out nuint loadedAssemblies))
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.LoadedAssembliesNotResolved);
        }

        return new(
            IsSuccess: true,
            Value: loadedAssemblies);
    }

    public virtual Result<nuint, MonoInitializationError> FindDefaults(IMonoMemoryManager memory)
    {
        if (!memory.MonoModule.Symbols.TryGetValue("mono_get_corlib", out var symMonoGetCorLib)
            || symMonoGetCorLib.Address == 0)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.MonoDefaultsNotResolved);
        }

        Signature signature =
            memory.Is64Bit
            ? new(3, "48 8B 05")
            : new(1, "A1");

        nuint monoDefaultsRelative = memory.Scan(signature, symMonoGetCorLib.Address, 0x10);
        if (!memory.TryReadRelative(monoDefaultsRelative, out nuint monoDefaults))
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.MonoDefaultsNotResolved);
        }

        return new(
            IsSuccess: true,
            Value: monoDefaults);
    }
}
