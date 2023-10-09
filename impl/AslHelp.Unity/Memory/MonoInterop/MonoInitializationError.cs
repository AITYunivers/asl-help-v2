using AslHelp.Common.Results;

namespace AslHelp.Unity.Memory.MonoInterop;

public enum MonoInitializationError
{
    [ErrorMessage("Failure during struct initialization.")]
    StructInitializationFailed,

    [ErrorMessage("The provided Mono module is not supported.")]
    InvalidMonoModule,

    [ErrorMessage("The provided Il2Cpp version is not supported.")]
    UnsupportedIl2CppVersion,

    [ErrorMessage("The symbol 'mono_assembly_foreach' was not found in the Mono module.")]
    SymbolMonoAssemblyForeachNotFound,

    [ErrorMessage("The address for 'mono_assembly_foreach' was null.")]
    SymbolMonoAssemblyForeachNull,

    [ErrorMessage("None of the provided signatures for 'mono_assembly_foreach' could be resolved.")]
    MonoAssemblyForeachSignatureNotResolved,

    [ErrorMessage("The relative address for 'mono_assembly_foreach' could not be read.")]
    MonoAssemblyForeachRelativeReadFailed
}
