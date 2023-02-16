using AslHelp.Core.Memory;

namespace AslHelp.Core.Collections;

public sealed class ModuleCache : CachedEnumerable<string, Module>
{
    private readonly Process _process;

    public ModuleCache(Process process)
        : base(StringComparer.OrdinalIgnoreCase)
    {
        _process = process;
    }

    public override IEnumerator<Module> GetEnumerator()
    {
        foreach (Module module in _process.ModulesTh32())
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
