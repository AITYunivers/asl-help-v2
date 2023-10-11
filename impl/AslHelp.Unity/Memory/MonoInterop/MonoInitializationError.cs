using AslHelp.Common.Results;

namespace AslHelp.Unity.Memory.MonoInterop;

public enum MonoInitializationError
{
    [ErrorMessage("Failure during struct initialization.")]
    StructInitializationFailed,

    [ErrorMessage("The provided Mono module is not supported.")]
    InvalidMonoModule,

    [ErrorMessage("This Il2Cpp version is not supported.")]
    UnsupportedIl2CppVersion,

    [ErrorMessage("The symbol 'mono_assembly_foreach' was not found in the Mono module.")]
    SymbolMonoAssemblyForeachNotFound,

    [ErrorMessage("The address for 'mono_assembly_foreach' was null.")]
    SymbolMonoAssemblyForeachNull,

    [ErrorMessage("None of the signatures for 'mono_assembly_foreach' could be resolved.")]
    MonoAssemblyForeachSignaturesNotResolved,

    [ErrorMessage("The relative address for 'mono_assembly_foreach' could not be read.")]
    MonoAssemblyForeachRelativeReadFailed,

    [ErrorMessage("None of the signatures for 's_Assemblies' could be resolved.")]
    SAssembliesSignaturesNotResolved,

    [ErrorMessage("The relative address for 's_Assemblies' could not be read.")]
    SAssembliesRelativeReadFailed,

    [ErrorMessage("None of the signatures for 'typeInfoDefinitionTable' could be resolved.")]
    TypeInfoDefinitionTableSignaturesNotResolved,

    [ErrorMessage("The relative address for 'typeInfoDefinitionTable' could not be read.")]
    TypeInfoDefinitionTableRelativeReadFailed
}
