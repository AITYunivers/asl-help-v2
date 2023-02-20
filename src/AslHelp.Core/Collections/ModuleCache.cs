using AslHelp.Core.Memory;

namespace AslHelp.Core.Collections;

public sealed class ModuleCache : CachedEnumerable<string, Module>
{
    private readonly int _processId;
    private readonly nint _processHandle;

    public ModuleCache(Process process)
        : this(process.Id, process.Handle) { }

    public ModuleCache(int processId, nint processHandle)
        : base(StringComparer.OrdinalIgnoreCase)
    {
        _processId = processId;
        _processHandle = processHandle;
    }

    public override IEnumerator<Module> GetEnumerator()
    {
        foreach (Module module in Native.ModulesTh32(_processHandle, _processId))
        {
            yield return module;
        }
    }

    protected override string GetKey(Module value)
    {
        return value.Name;
    }

    protected override string KeyNotFoundMessage(string key)
    {
        return $"The given module '{key}' was not present in the process.";
    }
}
