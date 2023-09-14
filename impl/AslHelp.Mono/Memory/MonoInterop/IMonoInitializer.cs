using AslHelp.Core.IO.Parsing;
using AslHelp.Mono.Memory.Ipc;

namespace AslHelp.Mono.Memory.MonoInterop;

public interface IMonoInitializer
{
    NativeStructMap InitializeStructs(bool is64Bit);
    nuint InitializeAssemblies(IMonoMemoryManager memory);
}
