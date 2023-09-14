using AslHelp.Core.IO.Parsing;
using AslHelp.Mono.Memory.Ipc;

namespace AslHelp.Mono.Memory.MonoInterop;

internal interface IMonoInitializer
{
    NativeStructMap InitializeStructs(bool is64Bit);
    nuint InitializeAssemblies(IMonoMemoryManager memory);
}
