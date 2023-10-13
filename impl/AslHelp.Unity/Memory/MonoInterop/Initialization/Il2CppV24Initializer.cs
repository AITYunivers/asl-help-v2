using System;

using AslHelp.Common.Results;
using AslHelp.Core.IO.Parsing;
using AslHelp.Core.Memory.SignatureScanning;
using AslHelp.Unity.Memory.Ipc;

namespace AslHelp.Unity.Memory.MonoInterop.Initialization;

public class Il2CppV24Initializer : IIl2CppInitializer
{
    public Result<NativeStructMap, MonoInitializationError> InitializeStructs(IMonoMemoryManager memory)
    {
        var result = NativeStructMap.InitializeFromResource("Unity", "il2cpp", "v24", memory.Is64Bit);
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

    public Result<nuint, MonoInitializationError> FindLoadedAssemblies(IMonoMemoryManager memory)
    {
        if (!memory.MonoModule.Symbols.TryGetValue("il2cpp_domain_get_assemblies", out var symIl2CppDomainGetAssemblies)
            || symIl2CppDomainGetAssemblies.Address == 0)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.SAssembliesNotResolved);
        }

        Signature callSignature =
            memory.Is64Bit
            ? new(1, "E8")
            : throw new NotImplementedException();

        Signature signature =
            memory.Is64Bit
            ? new(3, "48 8D 05")
            : throw new NotImplementedException();

        nuint getAssembliesRelative = memory.Scan(callSignature, symIl2CppDomainGetAssemblies.Address, 0x20);
        nuint sAssembliesRelative =
            memory.TryReadRelative(getAssembliesRelative, out nuint getAssemblies)
            ? memory.Scan(signature, getAssemblies, 0x10)
            : memory.Scan(signature, symIl2CppDomainGetAssemblies.Address, 0x20);

        if (!memory.TryReadRelative(sAssembliesRelative, out nuint sAssemblies))
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.SAssembliesNotResolved);
        }

        return new(
            IsSuccess: true,
            Value: sAssemblies);
    }

    public Result<nuint, MonoInitializationError> FindTypeInfoDefinitions(IMonoMemoryManager memory)
    {
        throw new NotImplementedException();
    }

    public Result<nuint, MonoInitializationError> FindDefaults(IMonoMemoryManager memory)
    {
        throw new NotImplementedException();
    }
}
