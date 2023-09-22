using AslHelp.Common.Exceptions;
using AslHelp.Core.Extensions;
using AslHelp.Core.IO.Parsing;
using AslHelp.Core.Memory.SignatureScanning;
using AslHelp.Mono.Memory.Ipc;

namespace AslHelp.Mono.Memory.MonoInterop.MonoV1;

internal sealed class MonoV1Initializer : IMonoInitializer
{
    public nuint InitializeAssemblies(IMonoMemoryManager memory)
    {
        if (!memory.MonoModule.Symbols.TryGetValue("mono_assembly_foreach", out var monoAssemblyForeach)
            || monoAssemblyForeach.Address == 0)
        {
            const string msg = "Unable to find symbol 'mono_assembly_foreach'.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        bool is64Bit = memory.Is64Bit;
        Signature[] signatures = is64Bit
            ? [new(3, "48 8B 0D")]
            : [new(2, "FF 35"), new(2, "8B 0D")];

        nuint loadedAssemblies = memory.Scan(signatures, monoAssemblyForeach.Address, 0x100);
        if (loadedAssemblies == 0)
        {
            const string msg = "Failed scanning for a reference to 'loaded_assemblies'.";
            ThrowHelper.ThrowInvalidOperationException(msg);
        }

        return memory.Read<nuint>(memory.ReadRelative(loadedAssemblies));
    }

    public NativeStructMap InitializeStructs(bool is64Bit)
    {
        return NativeStructMap.Parse("Mono", "mono", "v1", is64Bit);
    }
}