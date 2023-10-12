using AslHelp.Common.Results;

namespace AslHelp.Unity.Memory.MonoInterop;

public enum MonoInitializationError
{
    Unknown,

    // Startup errors.
    [ErrorMessage("The provided Mono module is not supported.")]
    InvalidMonoModule,

    [ErrorMessage("This Il2Cpp version is not supported.")]
    UnsupportedIl2CppVersion,

    // Struct initialization errors.
    [ErrorMessage("Failure during struct initialization.")]
    StructInitializationFailed,

    // Find loaded assemblies errors.
    [ErrorMessage("The symbol 'mono_assembly_foreach' was not found in the Mono module.")]
    SymbolMonoAssemblyForeachNotFound,

    [ErrorMessage("The address for 'mono_assembly_foreach' was null.")]
    SymbolMonoAssemblyForeachNull,

    // [ErrorMessage("None of the signatures for 'mono_assembly_foreach' could be resolved.")]

    // [ErrorMessage("The relative address for 'mono_assembly_foreach' could not be read.")]

    // s_Assemblies errors.
    [ErrorMessage("None of the signatures for 's_Assemblies' could be resolved.")]
    SAssembliesSignaturesNotResolved,

    [ErrorMessage("The relative address for 's_Assemblies' could not be read.")]
    SAssembliesRelativeReadFailed,

    // s_TypeInfoDefinitionTable errors.
    [ErrorMessage("None of the signatures for 's_TypeInfoDefinitionTable' could be resolved.")]
    STypeInfoDefinitionTableSignaturesNotResolved,

    [ErrorMessage("The relative address for 's_TypeInfoDefinitionTable' could not be read.")]
    STypeInfoDefinitionTableRelativeReadFailed,

    // Defaults errors.
    [ErrorMessage("None of the signatures for 'mono_defaults'/'il2cpp_defaults' could be resolved.")]
    DefaultsSignaturesNotResolved,

    [ErrorMessage("The relative address for 'mono_defaults'/'il2cpp_defaults' could not be read.")]
    DefaultsRelativeReadFailed,

    [ErrorMessage("The fields of the defaults could not be read.")]
    DefaultsFieldsReadFailed
}
