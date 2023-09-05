using AslHelp.Core.Memory;
using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Mono.Memory.Ipc;

public interface IMonoMemoryManager :
    IMemoryManager,
    IMonoMemoryReader,
    IMonoMemoryWriter
{
    Module MonoModule { get; }
}
