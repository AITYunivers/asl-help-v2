using AslHelp.Core.IO.Parsing;
using AslHelp.Mono.Memory.Ipc;

namespace AslHelp.Mono.Memory.MonoInterop;

internal interface IMonoInitializer
{
    NativeStructMap InitializeStructs(IMonoMemoryManager memory);
    nuint InitializeAssemblies(IMonoMemoryManager memory);
}
