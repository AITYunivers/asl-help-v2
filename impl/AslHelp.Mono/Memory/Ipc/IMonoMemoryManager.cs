using AslHelp.Core.Memory.Ipc;

namespace AslHelp.Mono.Memory.Ipc;

public interface IMonoMemoryManager :
    IMemoryManager,
    IMonoMemoryReader,
    IMonoMemoryWriter
{ }
