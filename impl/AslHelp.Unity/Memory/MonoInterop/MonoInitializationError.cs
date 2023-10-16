using AslHelp.Common.Results;

namespace AslHelp.Unity.Memory.MonoInterop;

public enum MonoInitializationError
{
    Unknown,

    [ErrorMessage("This Mono module is unsupported.")]
    UnsupportedMonoModule,

    [ErrorMessage("This Il2Cpp version is not supported.")]
    UnsupportedIl2CppVersion,

    [ErrorMessage("Failure during struct initialization.")]
    StructInitializationFailed,

    [ErrorMessage("The symbol 'mono_assembly_foreach' was not found in the Mono module.")]
    SymbolMonoAssemblyForeachNotFound,

    [ErrorMessage("'loaded_assemblies'/'s_Assemblies' could not be resolved.")]
    LoadedAssembliesNotResolved,

    [ErrorMessage("'s_Assemblies' could not be resolved.")]
    SAssembliesNotResolved,

    [ErrorMessage("'s_TypeInfoDefinitionTable' could not be resolved.")]
    STypeInfoDefinitionTableNotResolved,

    [ErrorMessage("'mono_defaults' could not be resolved.")]
    MonoDefaultsNotResolved,

    [ErrorMessage("'il2cpp_defaults' could not be resolved.")]
    Il2CppDefaultsNotResolved
}
