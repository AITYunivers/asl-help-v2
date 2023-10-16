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
            : new(1, "E8");

        Signature signatureFromIl2CppDomainGetAssemblies =
            memory.Is64Bit
            ? new(3, "48 2B 05")
            : new(1, "2B 0D");

        Signature signatureFromGetAssemblies =
            memory.Is64Bit
            ? new(3, "48 8D 05")
            : new(1, "B8");

        nuint getAssembliesRelative = memory.Scan(callSignature, symIl2CppDomainGetAssemblies.Address, 0x20);
        nuint sAssembliesRelative =
            memory.TryReadRelative(getAssembliesRelative, out nuint getAssemblies)
            ? memory.Scan(signatureFromGetAssemblies, getAssemblies, 0x10)
            : memory.Scan(signatureFromIl2CppDomainGetAssemblies, symIl2CppDomainGetAssemblies.Address, 0x20);

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
        Signature signature =
            memory.Is64Bit
            ? new(11, "48 8D 1C FD 00000000 48 8B 05")
            : new(5, "8B E5 5D C3 A1 ???????? 83 3C ?? 00");

        nuint sTypeInfoDefinitionsRelative = memory.Scan(signature, memory.MonoModule);
        if (!memory.TryReadRelative(sTypeInfoDefinitionsRelative, out nuint sTypeInfoDefinitions))
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.STypeInfoDefinitionTableNotResolved);
        }

        return new(
            IsSuccess: true,
            Value: sTypeInfoDefinitions);
    }

    public Result<nuint, MonoInitializationError> FindDefaults(IMonoMemoryManager memory)
    {
        if (!memory.MonoModule.Symbols.TryGetValue("il2cpp_get_corlib", out var symIl2CppGetCorlib)
            || symIl2CppGetCorlib.Address == 0)
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.Il2CppDefaultsNotResolved);
        }

        Signature callSignature =
            memory.Is64Bit
            ? new(1, "E9")
            : new(1, "E9");

        Signature signatureFromil2CppGetCorlib =
            memory.Is64Bit
            ? new(3, "48 8B 05")
            : new(1, "A1");

        nuint il2CppGetCorlibRelative = memory.Scan(callSignature, symIl2CppGetCorlib.Address, 0x10);
        nuint il2CppDefaultsRelative =
            memory.TryReadRelative(il2CppGetCorlibRelative, out nuint il2CppGetCorlib)
            ? memory.Scan(signatureFromil2CppGetCorlib, il2CppGetCorlib, 0x10)
            : memory.Scan(signatureFromil2CppGetCorlib, symIl2CppGetCorlib.Address, 0x10);

        if (!memory.TryReadRelative(il2CppDefaultsRelative, out nuint il2CppDefaults))
        {
            return new(
                IsSuccess: false,
                Error: MonoInitializationError.Il2CppDefaultsNotResolved);
        }

        return new(
            IsSuccess: true,
            Value: il2CppDefaults);
    }
}
