namespace AslHelp.Core.Memory.Ipc;

public interface IMemoryInjector
{
    bool IsInjected(string dllPath);
    bool TryInjectDll(string dllPath, string? entryPoint = null);
    uint
}
